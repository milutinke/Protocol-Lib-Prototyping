namespace ProtocolLibraryPrototype.Protocol.Attributes;

/*
 * This is a definition for the PacketMeta attribute that we use to specify the meta data
 * of a packet, which helps us to find an appropriate packet for a specific protocol version
 */
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PacketMeta : Attribute
{
    public Versions protocolVersion { get; }
    public int packetId { get; }

    public PacketMeta(Versions protocolVersion, int packetId)
    {
        this.protocolVersion = protocolVersion;
        this.packetId = packetId;
    }
}