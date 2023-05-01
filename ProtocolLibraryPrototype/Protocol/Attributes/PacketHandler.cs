using ProtocolLibraryPrototype.Protocol.Packets;

namespace ProtocolLibraryPrototype.Protocol.Attributes;

/*
 * This is a definition for the PacketHandler attribute which we use to specify meta data
 * on packet handlers
 */
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PacketHandler : Attribute
{
    public Versions protocolVersion;
    public PacketTypes packetType;
    
    public PacketHandler(Versions protocolVersion, PacketTypes packetType)
    {
        this.protocolVersion = protocolVersion;
        this.packetType = packetType;
    }
}