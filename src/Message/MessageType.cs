//-----------------------------------------------------------------------
// <copyright file="MessageType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    /// <summary>Enumeration for frame types.</summary>
    public enum FrameType
    {
        /// <summary>First start frame.</summary>
        Start = 0x00,

        /// <summary>Client hello frame.</summary>
        ClientHello = 0x01,

        /// <summary>Server hello frame.</summary>
        ServerHello = 0x02,

        /// <summary>Client ping request.</summary>
        ClientPing = 0x03,

        /// <summary>Server pong response.</summary>
        ServerPong = 0x04,

        /// <summary>Server ping request.</summary>
        ServerPing = 0x05,

        /// <summary>Client pong response.</summary>
        ClientPong = 0x06,

        /// <summary>Containing message with commands.</summary>
        Message = 0x07,
    }
}
