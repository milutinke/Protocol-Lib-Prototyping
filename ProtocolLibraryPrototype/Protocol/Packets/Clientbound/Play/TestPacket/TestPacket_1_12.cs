using ProtocolLibraryPrototype.Packets;
using ProtocolLibraryPrototype.Protocol;
using ProtocolLibraryPrototype.Protocol.Attributes;

namespace TestReflection.Protocol.Packets.Play.TestPacket
{
    [PacketMeta(ProtocolVersion: Versions.MC_1_12, PacketId: 0x3)]
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
}