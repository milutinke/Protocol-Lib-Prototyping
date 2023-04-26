using ProtocolLibraryPrototype.Packets;
using ProtocolLibraryPrototype.Protocol.Attributes;

namespace ProtocolLibraryPrototype.Protocol
{
    public class PacketRegistry
    {
        public static Dictionary<int, Type> LoginPackets = new Dictionary<int, Type>();
        public static Dictionary<int, Type> InboundPackets = new Dictionary<int, Type>();
        public static Dictionary<int, Type> OutboundPackets = new Dictionary<int, Type>();

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
                var packetTypes = packet.GetCustomAttributes(typeof(PacketType), false);
                var packetTypeAttribute = packetTypes.Length == 1 ? (PacketType)packetTypes.First() : null;

                if (packetTypeAttribute == null)
                {
                    // TODO: War about a packet that doesn't have a packet type attribute
                    continue;
                }

                // Get the packe meta attribute for the specified protocol version so we can get a specific packet ID for that protocol version
                var packetMeta = packet.GetCustomAttributes(typeof(PacketMeta), false)
                    .Where(m => ((PacketMeta)m).ProtocolVersion == protocolVersion)
                    .Select(m => (PacketMeta)m)
                    .First();

                if (packetMeta == null)
                    continue;

                var packetId = packetMeta.PacketId;

                if (packetTypeAttribute.Type == PacketTypes.Login)
                    LoginPackets.Add(packetMeta.PacketId, packet);
                else if (packetTypeAttribute.Type == PacketTypes.In)
                    InboundPackets.Add(packetMeta.PacketId, packet);
                else if (packetTypeAttribute.Type == PacketTypes.Out)
                    OutboundPackets.Add(packetMeta.PacketId, packet);
            }
        }

        public static void UnregisterPackets()
        {
            LoginPackets.Clear();
            InboundPackets.Clear();
            OutboundPackets.Clear();
        }
    }
}