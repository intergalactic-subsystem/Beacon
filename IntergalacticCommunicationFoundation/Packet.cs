using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IntergalacticCommunicationFoundation
{
    [Serializable]
    [DataContract]
    public sealed class Packet : IPacket
    {
        [DataMember(EmitDefaultValue = false, IsRequired = true)]
        public Guid Id { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = true)]
        public string StoredId { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = true)]
        public string Message { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = true)]
        public byte[] Payload { get; set; }

        [DataMember(IsRequired = true, EmitDefaultValue = false)]
        public Guid[] Chain { get; set; }

        [DataMember(IsRequired = true)]
        public Guid From { get; set; }

        [DataMember(IsRequired = true)]
        public Guid To { get; set; }

        [DataMember(IsRequired = true, EmitDefaultValue = false)]
        public DateTime TransmissionTime { get; set; }
    }
}
