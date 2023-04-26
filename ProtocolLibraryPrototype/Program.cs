using ProtocolLibraryPrototype.Protocol;

namespace ProtocolLibraryPrototype
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Versions protocolVersion = Versions.MC_1_12;
            PacketRegistry.RegisterPackets(protocolVersion);

            byte somePacketData = 0x23;
            int packetId = 0x3;
            var packetReadingSimulator = new SimulateReadPacket(somePacketData, packetId);

            packetReadingSimulator.PacketReadEvent += packet =>
            {
                var converted = Convert.ChangeType(packet, packet.GetType());
                var packetProperties = converted.GetType().GetProperties().ToList();
                var packetFields = converted.GetType().GetFields();
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