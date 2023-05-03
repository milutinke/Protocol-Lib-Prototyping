using ProtocolLibraryPrototype.Protocol.Packet;
using ProtocolLibraryPrototype.Protocol.Packets;
using ProtocolLibraryPrototype.Protocol.Registries;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProtocolLibraryPrototype.Protocol
{
    public class SimulatePacketSend
    {
        private static BindingFlags FLAGS = BindingFlags.Default | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static void SendPacket(PacketTypes packetType, object packetData)
        {
            Type? foundClass = null;

            // Find a Packet Class that matches the packet type
            foreach (var currentPacketType in PacketRegistry.ServerboundPackets.Values)
            {
                string packetNameSterilized = Regex.Replace(currentPacketType.Name, @"[^a-zA-Z]+", String.Empty).Trim();
                if (Enum.TryParse(packetNameSterilized, false, out PacketTypes type))
                {
                    if (packetType.Equals(type))
                    {
                        foundClass = currentPacketType;
                        break;
                    }
                }
            }

            if (foundClass != null)
            {
                Console.WriteLine("Found type");
                var packet = (ServerboundPacket)Activator.CreateInstance(foundClass)!;

                if (packet == null)
                {
                    Console.WriteLine("Failed to create a packet instance!");
                    return;
                }

                foreach (var packetDataProperty in packetData.GetType().GetProperties())
                {
                    foreach (var packetField in packet.GetType().GetFields(FLAGS))
                    {
                        if (packetField.Name.ToLower().Equals(packetDataProperty.Name.ToLower()))
                        {
                            if (packetField.FieldType != packetDataProperty.PropertyType)
                            {
                                Console.WriteLine($"Packet data property {packetDataProperty.Name} matches the name of field {packetField.Name} on {foundClass.Name} but has a different type of value ({packetDataProperty.PropertyType} != {packetField.FieldType})");
                                continue;
                            }

                            packetField.SetValue(packet, packetDataProperty.GetValue(packetData));
                        }
                    }

                    foreach (var packetProperty in packet.GetType().GetProperties(FLAGS))
                    {
                        if (packetProperty.Name.ToLower().Equals(packetDataProperty.Name.ToLower()))
                        {
                            if (!packetProperty.CanWrite)
                            {
                                Console.WriteLine($"Packet data property {packetDataProperty.Name} matches the name of property {packetProperty.Name} on {foundClass.Name} but {packetProperty.Name} can not be set because it does not have a setter!");
                                continue;
                            }

                            if (packetProperty.PropertyType != packetDataProperty.PropertyType)
                            {
                                Console.WriteLine($"Packet data property {packetDataProperty.Name} matches the name of property {packetProperty.Name} on {foundClass.Name} but has a different type of value ({packetDataProperty.PropertyType} != {packetProperty.PropertyType})");
                                continue;
                            }

                            packetProperty.SetValue(packet, packetDataProperty.GetValue(packetData));
                        }
                    }
                }

                packet.Write();
                Console.WriteLine($"Packet data: {ByteArrayToString(packet.GetPacketData().ToArray())}");
            }
            else Console.WriteLine($"Failed to find a class of a packet type: {packetType}");
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", " ");
        }

    }
}
