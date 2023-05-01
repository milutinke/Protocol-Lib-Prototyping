using ProtocolLibraryPrototype.Protocol.Attributes;
using ProtocolLibraryPrototype.Protocol.Packet;

namespace ProtocolLibraryPrototype.Protocol.Packets.Definitions.Clientbound.Play.TestPacket;

[PacketMeta(protocolVersion: Versions.MC_1_12, packetId: 0x3)]
public class TestPacket_1_12 : ClientboundPacket
{
    public int SomeField { get; set; }
    public byte SomeOtherField { get; set; }
    public int SomeNewField { get; set; }

    public override void Read()
    {
        SomeField = ReadVarInt();
        SomeOtherField = ReadByte();
        SomeNewField = ReadVarInt();
    }
}