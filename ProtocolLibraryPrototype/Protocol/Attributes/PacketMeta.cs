namespace ProtocolLibraryPrototype.Protocol.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PacketMeta : Attribute
    {
        public Versions protocolVersion { get; }
        public int packetId { get; }

        public PacketMeta(Versions protocolVersion, int packetId)
        {
            this.protocolVersion = protocolVersion;
            this.packetId = packetId;
        }
    }
}
