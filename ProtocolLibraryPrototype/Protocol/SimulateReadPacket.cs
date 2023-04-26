using ProtocolLibraryPrototype.Packets;

namespace ProtocolLibraryPrototype.Protocol
{
    public class SimulateReadPacket
    {
        public delegate void OnPacketRead(Packet packet);
        public event OnPacketRead? PacketReadEvent;

        private byte somePacketData;
        private int packetId;

        public SimulateReadPacket(byte somePacketData, int packetId)
        {
            this.somePacketData = somePacketData;
            this.packetId = packetId;
        }

        public void ReadPacket()
        {
            var type = PacketRegistry.InboundPackets[packetId];

            if (type == null)
                return;

            var packet = (Packet)Activator.CreateInstance(type)!;
            packet.PacketData.Enqueue(somePacketData);
            packet.Read();
            PacketReadEvent?.Invoke(packet);
        }
    }
}
