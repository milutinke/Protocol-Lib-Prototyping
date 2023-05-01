using ProtocolLibraryPrototype.Protocol.Attributes;
using ProtocolLibraryPrototype.Protocol.Packet;

namespace ProtocolLibraryPrototype.Protocol.Registries
{
    /*
     * This class holds a list of packets that are sent to the client (clientbound), and that are supposed to be sent to the server (serverbound)
     * We scan the assembly for BasePacket type and PacketMeta annotation, then we use ClientboundPacket and ServerboundPacket
     * to differentiate and sort the packets accordingly
     */
    public class PacketRegistry
    {
        public static Dictionary<int, Type> _clienboundPackets = new();
        public static Dictionary<int, Type> _serverboundPackets = new();

        public static Dictionary<int, Type> ClientboundPackets
        {
            get { return _clienboundPackets; }
        }
        
        public static Dictionary<int, Type> ServerboundPackets
        {
            get { return _serverboundPackets; }
        }

        public static void RegisterPackets(Versions protocolVersion)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName!.StartsWith(nameof(ProtocolLibraryPrototype)));

            // Get a list of packets that have the PacketMeta attribute and are for the given protocol version
            var packets = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(BasePacket).IsAssignableFrom(t))
                .Where(t => t.GetCustomAttributes(typeof(PacketMeta), false)
                .Any(a => ((PacketMeta)a).protocolVersion == protocolVersion))
                .ToList();

            foreach (var packet in packets)
            {
                // Get the packet meta attribute for the specified protocol version so we can get a specific packet ID for the given protocol version
                var packetId = packet.GetCustomAttributes(typeof(PacketMeta), false)
                    .Where(m => ((PacketMeta)m).protocolVersion == protocolVersion)
                    .Select(m => (PacketMeta)m)
                    .First().packetId;
                
                if(packet.IsSubclassOf(typeof(ClientboundPacket)))
                    _clienboundPackets.Add(packetId, packet);
                else if(packet.IsSubclassOf(typeof(ServerboundPacket)))
                    _serverboundPackets.Add(packetId, packet);
            }
        }

        public static void UnregisterPackets()
        {
            _clienboundPackets.Clear();
            _serverboundPackets.Clear();
        }
    }
}