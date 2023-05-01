using System.Reflection.Metadata;

namespace ProtocolLibraryPrototype.Protocol.Packet;

/*
 * Handler is a class that should serve as a bridge between a Packet class and the client code
 * It should process fields we got from the packet, it uses the Handle method to process the packet data,
 * and then relays it to the client instance
 * We can register multiple handlers (that handle different protocol version) for a packet type
 */
public abstract class Handler
{
    // TODO: Add a reference to the client class

    public abstract void Handle(BasePacket packet);
}