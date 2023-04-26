namespace ProtocolLibraryPrototype.Packets
{
    public abstract class Packet
    {
        public Queue<byte> PacketData = new Queue<byte>();

        public abstract void Read();
        public abstract void Write();

        public int ReadVarInt()
        {
            return 0;
        }

        public byte ReadByte()
        {
            return 0;
        }
    }
}
