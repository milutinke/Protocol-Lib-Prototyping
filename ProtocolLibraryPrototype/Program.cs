using ProtocolLibraryPrototype.Protocol;

namespace ProtocolLibraryPrototype
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Versions protocolVersion = Versions.MC_1_12;
            PacketRegistry.RegisterPackets(protocolVersion);

            byte[] somePacketData = { 0x23, 0x12, 0x12, 0x1, 0x02, 0xA, 0xB1, 0x11, 0x7 };
            int packetId = 0x3;
            var packetReadingSimulator = new SimulateReadPacket(somePacketData, packetId);

            packetReadingSimulator.PacketReadEvent += packet =>
            {
                var converted = Convert.ChangeType(packet, packet.GetType());
                Console.WriteLine(converted.GetType().Name);
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
            };

            packetReadingSimulator.ReadPacket();
        }
    }
}