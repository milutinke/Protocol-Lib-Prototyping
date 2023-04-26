using ProtocolLibraryPrototype.Packets;
using ProtocolLibraryPrototype.Protocol.Attributes;

namespace ProtocolLibraryPrototype.Protocol.Packets.Play.TestPacket
{
    [PacketMeta(ProtocolVersion: Versions.MC_1_8, PacketId: 0x1)]
    [PacketMeta(ProtocolVersion: Versions.MC_1_9, PacketId: 0x2)]
    [PacketType(PacketTypes.In)]
    public class TestPacket_1_8 : Packet
    {
        public int SomeField { get; set; }
        public byte SomeOtherField { get; set; }

        public override void Read()
        {
            SomeField = ReadVarInt();
            SomeOtherField = ReadByte();
        }

        public override void Write()
        {
        }
    }
}
