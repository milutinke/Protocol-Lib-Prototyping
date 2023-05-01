using ProtocolLibraryPrototype.Protocol.Packets;

namespace ProtocolLibraryPrototype.Protocol.Attributes;

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