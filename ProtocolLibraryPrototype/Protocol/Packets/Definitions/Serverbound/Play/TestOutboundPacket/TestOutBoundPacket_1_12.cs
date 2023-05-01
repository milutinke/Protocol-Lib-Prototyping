using ProtocolLibraryPrototype.Protocol.Attributes;
using ProtocolLibraryPrototype.Protocol.Packet;

namespace ProtocolLibraryPrototype.Protocol.Packets.Definitions.Serverbound.Play.TestOutboundPacket;

[PacketMeta(protocolVersion: Versions.MC_1_12, packetId: 0x1)]
public class TestOutBoundPacket_1_12 : ServerboundPacket
{
    public override void Write()
    {
        WriteString("Test");
        WriteVarInt(133);
    }
}