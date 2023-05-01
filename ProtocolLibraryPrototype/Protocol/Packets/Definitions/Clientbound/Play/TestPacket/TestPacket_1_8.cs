using ProtocolLibraryPrototype.Protocol.Attributes;
using ProtocolLibraryPrototype.Protocol.Packet;

namespace ProtocolLibraryPrototype.Protocol.Packets.Definitions.Clientbound.Play.TestPacket;

[PacketMeta(protocolVersion: Versions.MC_1_8, packetId: 0x1)]
[PacketMeta(protocolVersion: Versions.MC_1_9, packetId: 0x2)]
public class TestPacket_1_8 : ClientboundPacket
{
    public int SomeField { get; set; }
    public byte SomeOtherField { get; set; }

    public override void Read()
    {
        SomeField = ReadVarInt();
        SomeOtherField = ReadByte();
    }
}