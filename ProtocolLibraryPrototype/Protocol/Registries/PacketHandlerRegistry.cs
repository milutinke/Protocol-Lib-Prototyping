using ProtocolLibraryPrototype.Protocol.Attributes;
using ProtocolLibraryPrototype.Protocol.Packet;
using ProtocolLibraryPrototype.Protocol.Packets;

namespace ProtocolLibraryPrototype.Protocol.Registries
{
    /*
     * This class basically holds a list of handlers for the current protocol version we are working with
     * We are loading handlers using assembly scanning, we're looking for PacketHandler attribute/annotation
     * Handler is a class that should serve as a bridge between a Packet class and the client code
     * It should process fields we got from the packet.
     * We can register multiple handlers (that handle different protocol version) for a packet type
     */
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

            // Get a list of handlers that have the PacketHandler attribute and are for the given protocol version
            var handlers = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(Handler).IsAssignableFrom(t))
                .Where(t => t.GetCustomAttributes(typeof(PacketHandler), false)
                .Any(a => ((PacketHandler)a).protocolVersion == protocolVersion))
                .ToList();

            foreach (var handler in handlers)
            {
                // Get the PacketHandler attribute for the specified protocol version so we can get a specific packet type for the given protocol version
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