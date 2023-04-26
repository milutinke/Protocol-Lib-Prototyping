namespace ProtocolLibraryPrototype.Protocol.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PacketMeta : Attribute
    {
        public Versions ProtocolVersion { get; }
        public int PacketId { get; }

        public PacketMeta(Versions ProtocolVersion, int PacketId)
        {
            this.ProtocolVersion = ProtocolVersion;
            this.PacketId = PacketId;
        }
    }
}
