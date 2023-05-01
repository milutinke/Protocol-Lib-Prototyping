using ProtocolLibraryPrototype.Protocol.Attributes;
using ProtocolLibraryPrototype.Protocol.Packet;
using ProtocolLibraryPrototype.Protocol.Packets;

namespace ProtocolLibraryPrototype.Protocol.Registries
{
    public class PacketHandlerRegistry
    {
        public static Dictionary<PacketTypes, Type> _handlers = new();

        public static Dictionary<PacketTypes, Type> Handlers
        {
            get { return _handlers; }
        }

        public static void RegisterHandlers(Versions protocolVersion)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName!.StartsWith(nameof(ProtocolLibraryPrototype)));

            // Get a list of packets that have the PacketMeta attribute and are for the specified protocol version
            var handlers = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(Handler).IsAssignableFrom(t))
                .Where(t => t.GetCustomAttributes(typeof(PacketHandler), false)
                .Any(a => ((PacketHandler)a).protocolVersion == protocolVersion))
                .ToList();

            foreach (var handler in handlers)
            {
                // Get the packe meta attribute for the specified protocol version so we can get a specific packet ID for that protocol version
                var packetType = handler.GetCustomAttributes(typeof(PacketHandler), false)
                    .Where(h => ((PacketHandler)h).protocolVersion == protocolVersion)
                    .Select(h => (PacketHandler)h)
                    .First().packetType;
                
                _handlers.Add(packetType, handler);
            }
        }

        public static void UnregisterHandlers()
        {
            _handlers.Clear();
        }
    }
}