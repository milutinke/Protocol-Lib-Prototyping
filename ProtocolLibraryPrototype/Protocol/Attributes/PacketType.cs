using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLibraryPrototype.Protocol.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketType : Attribute
    {
        public PacketTypes Type { get; }
        public PacketType(PacketTypes Type)
        {
            this.Type = Type;
        }
    }
}
