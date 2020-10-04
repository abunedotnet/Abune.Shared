//-----------------------------------------------------------------------
// <copyright file="ServerPongMessage.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System;
    using System.IO;

    /// <summary>Server pong message.</summary>
    public class ServerPongMessage
    {
        /// <summary>Initializes a new instance of the <see cref="ServerPongMessage" /> class.</summary>
        public ServerPongMessage()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ServerPongMessage" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public ServerPongMessage(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.ClientRequestTimestamp = TimeSpan.FromTicks(br.ReadInt64());
                    this.ClientResponseTimestamp = TimeSpan.FromTicks(br.ReadInt64());
                    this.ServerTimestamp = TimeSpan.FromTicks(br.ReadInt64());
                }
            }
        }

        /// <summary>Gets or sets the client request timestamp.</summary>
        /// <value>The client request timestamp.</value>
        public TimeSpan ClientRequestTimestamp { get; set; }

        /// <summary>Gets or sets the client response timestamp.</summary>
        /// <value>The client response timestamp.</value>
        public TimeSpan ClientResponseTimestamp { get; set; }

        /// <summary>Gets or sets the server timestamp.</summary>
        /// <value>The server timestamp.</value>
        public TimeSpan ServerTimestamp { get; set; }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized instance.</returns>
        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(long) * 2))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ClientRequestTimestamp.Ticks);
                    bw.Write(this.ClientResponseTimestamp.Ticks);
                    bw.Write(this.ServerTimestamp.Ticks);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
