//-----------------------------------------------------------------------
// <copyright file="MessageType.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716
namespace Abune.Shared.Message.Contract
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

        /// <summary>The server authentication request.</summary>
        ServerAuthenticationRequest = 0x03,

        /// <summary>The client authentication response.</summary>
        ClientAuthenticationResponse = 0x04,

        /// <summary>Client ping request.</summary>
        ClientPing = 0x05,

        /// <summary>Server pong response.</summary>
        ServerPong = 0x06,

        /// <summary>Server ping request.</summary>
        ServerPing = 0x07,

        /// <summary>Client pong response.</summary>
        ClientPong = 0x08,

        /// <summary>Containing message with commands.</summary>
        Message = 0x09,
    }
}
