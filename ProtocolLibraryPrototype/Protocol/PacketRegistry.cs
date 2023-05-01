using ProtocolLibraryPrototype.Packets;
using ProtocolLibraryPrototype.Protocol.Attributes;

namespace ProtocolLibraryPrototype.Protocol
{
    public class PacketRegistry
    {
        public static Dictionary<int, Type> ClientboundPackets = new Dictionary<int, Type>();
        public static Dictionary<int, Type> ServerboundPackets = new Dictionary<int, Type>();

        public static void RegisterPackets(Versions protocolVersion)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName!.StartsWith(nameof(ProtocolLibraryPrototype)));

            // Get a list of packets that have the PacketMeta attribute and are for the specified protocol version
            var packets = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(Packet).IsAssignableFrom(t))
                .Where(t => t.GetCustomAttributes(typeof(PacketMeta), false)
                .Any(a => ((PacketMeta)a).ProtocolVersion == protocolVersion))
                .ToList();

            foreach (var packet in packets)
            {
                // Get the packe meta attribute for the specified protocol version so we can get a specific packet ID for that protocol version
                var packetId = packet.GetCustomAttributes(typeof(PacketMeta), false)
                    .Where(m => ((PacketMeta)m).ProtocolVersion == protocolVersion)
                    .Select(m => (PacketMeta)m)
                    .First().PacketId;
                
                if(packet.IsSubclassOf(typeof(ClientboundPacket)))
                    ClientboundPackets.Add(packetId, packet);
                else if(packet.IsSubclassOf(typeof(ServerboundPacket)))
                    ServerboundPackets.Add(packetId, packet);
            }
        }

        public static void UnregisterPackets()
        {
            ClientboundPackets.Clear();
            ServerboundPackets.Clear();
        }
    }
}