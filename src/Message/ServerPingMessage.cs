//-----------------------------------------------------------------------
// <copyright file="ServerPingMessage.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System;
    using System.IO;

    /// <summary>Message for server ping.</summary>
    public class ServerPingMessage
    {
        /// <summary>Initializes a new instance of the <see cref="ServerPingMessage" /> class.</summary>
        public ServerPingMessage()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ServerPingMessage" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public ServerPingMessage(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.ServerTimestamp = TimeSpan.FromTicks(br.ReadInt64());
                }
            }
        }

        /// <summary>Gets or sets the server timestamp.</summary>
        /// <value>The server timestamp.</value>
        public TimeSpan ServerTimestamp { get; set; }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized instance.</returns>
        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(long)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ServerTimestamp.Ticks);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
