using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntergalacticCommunicationFoundation
{
    public interface IPacket
    {
        /// <summary>
        /// The system internal id
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The committed sha, if empty, not stored yet
        /// </summary>
        string StoredId { get; set; }

        /// <summary>
        /// The message being transmitted
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Any binary payload
        /// </summary>
        byte[] Payload { get; set; }

        /// <summary>
        /// The chain of beacons
        /// </summary>
        Guid[] Chain { get; set; }

        /// <summary>
        /// Id of the sender
        /// </summary>
        Guid From { get; set; }

        /// <summary>
        /// Id of whom the message is addressed to
        /// </summary>
        Guid To { get; set; }

        /// <summary>
        /// Initial time transmission was received by the first beacon
        /// </summary>
        DateTime TransmissionTime { get; set; }
    }
}
