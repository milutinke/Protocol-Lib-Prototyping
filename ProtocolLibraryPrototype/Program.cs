using System.Text.RegularExpressions;
using ProtocolLibraryPrototype.Protocol;
using ProtocolLibraryPrototype.Protocol.Packet;
using ProtocolLibraryPrototype.Protocol.Packets;
using ProtocolLibraryPrototype.Protocol.Registries;

namespace ProtocolLibraryPrototype
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Versions protocolVersion = Versions.MC_1_12;
            PacketRegistry.RegisterPackets(protocolVersion);
            PacketHandlerRegistry.RegisterHandlers(protocolVersion);

            byte[] somePacketData = { 0x23, 0x12, 0x12, 0x1, 0x02, 0xA, 0xB1, 0x11, 0x7 };
            int packetId = 0x3;
            var packetReadingSimulator = new SimulateReadPacket(somePacketData, packetId);

            packetReadingSimulator.PacketReadEvent += packet =>
            {
                var converted = Convert.ChangeType(packet, packet.GetType());
                string packetNameSterilized = Regex.Replace(converted.GetType().Name, @"[^a-zA-Z]+", String.Empty).Trim();

                if (Enum.TryParse(packetNameSterilized, false, out PacketTypes packetType))
                {
                    Console.WriteLine($"Found type: {packetType}");

                    if (PacketHandlerRegistry.Handlers.ContainsKey(packetType))
                    {
                        var handler = (Handler)Activator.CreateInstance(PacketHandlerRegistry.Handlers[packetType])!;
                        handler.Handle(packet);
                    }
                }
                else Console.WriteLine("Could not find the type!");
            };

            packetReadingSimulator.ReadPacket();
        }
    }
}