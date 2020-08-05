//-----------------------------------------------------------------------
// <copyright file="MessageControlFlags.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CA1716

namespace Abune.Shared.Protocol
{
    using System;

    /// <summary>Control flags for message header.</summary>
    [Flags]
    #pragma warning disable CA1028
    public enum MessageControlFlags : byte
    {
        /// <summary>
        /// No Operation - ‭‬0b0000_0000
        /// followed by [].
        /// </summary>
        NOOP = 0x0,

        /// <summary>
        /// Acknowledge (QoS 1) - 0b0000_0001
        /// followed by [ACK-MESSAGEID] [...].
        /// </summary>
        ACK = 0x1,

        /// <summary>
        /// Acknowledge Response (QoS 2) - 0b0000_0011
        ///  [...] followed by [ACK-ACK-MESSAGEID] [...].
        /// </summary>
        ACK2 = 0x3,

        /// <summary>
        /// Negative Acknoledgment (Qos 1, 2) - 0b0000_0100
        /// [...] followed by [NACK-MESSAGEID] [...].
        /// </summary>
        NACK = 0x4,

        /// <summary>
        /// Reject / Drop (Qos 1, 2) - 0b0000_1100
        /// [...] followed by [NACK-MESSAGEID] [...].
        /// </summary>
        REJ = 0xC,

        /// <summary>
        /// Quality of service "none" - 0b0000_0000
        /// [...] followed by [PAYLOAD].
        /// </summary>
        QOS0 = 0x0,

        /// <summary>
        /// Quality of service "at least once" - 0b0001_0000
        /// [...] followed by [PAYLOAD].
        /// </summary>
        QOS1 = 0x10,

        /// <summary>
        /// Quality of service "exactly once" - 0b0011_0000
        /// [...] followed by [PAYLOAD].
        /// </summary>
        QOS2 = 0x30,

        /// <summary>
        /// Message with command - 0b1000_0000
        /// followed by [...] [PAYLOAD].
        /// </summary>
        COMMAND = 0x80,
    }
}
