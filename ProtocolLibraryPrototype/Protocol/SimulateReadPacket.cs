using ProtocolLibraryPrototype.Packets;

namespace ProtocolLibraryPrototype.Protocol
{
    public class SimulateReadPacket
    {
        public delegate void OnPacketRead(Packet packet);

        public event OnPacketRead? PacketReadEvent;

        private byte[] somePacketData;
        private int packetId;

        public SimulateReadPacket(byte[] somePacketData, int packetId)
        {
            this.somePacketData = somePacketData;
            this.packetId = packetId;
        }

        public void ReadPacket()
        {
            if (!PacketRegistry.ClientboundPackets.ContainsKey(packetId))
            {
                // TODO: Warn about a not handled packet
                return;
            }
            
            var packet = (ClientboundPacket)Activator.CreateInstance(PacketRegistry.ClientboundPackets[packetId])!;
            packet.SetPacketData(somePacketData);
            packet.Read();
            PacketReadEvent?.Invoke(packet);
        }
    }
}