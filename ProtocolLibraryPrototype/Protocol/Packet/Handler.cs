using System.Reflection.Metadata;

namespace ProtocolLibraryPrototype.Protocol.Packet;

public abstract class Handler
{
    // TODO: Add a reference to the client class

    public abstract void Handle(BasePacket packet);
}