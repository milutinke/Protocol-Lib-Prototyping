using ProtocolLibraryPrototype.Protocol.Attributes;
using ProtocolLibraryPrototype.Protocol.Packet;
using ProtocolLibraryPrototype.Protocol.Packets.Definitions.Clientbound.Play.TestPacket;

namespace ProtocolLibraryPrototype.Protocol.Packets.Handlers.TestPacket;

[PacketHandler(Versions.MC_1_12, PacketTypes.TestPacket)]
public class TestPacketHandler_1_12 : Handler
{
    public override void Handle(BasePacket packet)
    {
        packet = (TestPacket_1_12)packet;
        Console.WriteLine($"Handling packer: " + packet.GetType().Name);
        
        var packetProperties = packet.GetType().GetProperties().ToList();
        var packetFields = packet.GetType().GetFields();
        foreach (var property in packetProperties)
        {
            Console.WriteLine($"{property.Name}: {property.GetValue(packet)}");
        }

        foreach (var field in packetFields)
        {
            Console.WriteLine($"{field.Name}: {field.GetValue(packet)}");
        }
    }
}