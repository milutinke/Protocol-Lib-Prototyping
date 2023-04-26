using ProtocolLibraryPrototype.Packets;
using ProtocolLibraryPrototype.Protocol;
using ProtocolLibraryPrototype.Protocol.Attributes;

namespace TestReflection.Protocol.Packets.Play.TestPacket
{
    [PacketMeta(ProtocolVersion: Versions.MC_1_12, PacketId: 0x3)]
    [PacketType(PacketTypes.In)]
    public class TestPacket_1_12 : Packet
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

        public override void Write()
        {
        }
    }
}
