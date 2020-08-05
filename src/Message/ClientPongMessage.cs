//-----------------------------------------------------------------------
// <copyright file="ClientPongMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System;
    using System.IO;

    /// <summary>Client pong response message.</summary>
    public class ClientPongMessage
    {
        /// <summary>Initializes a new instance of the <see cref="ClientPongMessage" /> class.</summary>
        public ClientPongMessage()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ClientPongMessage" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public ClientPongMessage(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.ServerRequestTimestamp = TimeSpan.FromTicks(br.ReadInt64());
                    this.ClientRequestTimestamp = TimeSpan.FromTicks(br.ReadInt64());
                    this.ClientResponseTimestamp = TimeSpan.FromTicks(br.ReadInt64());
                }
            }
        }

        /// <summary>Gets or sets the server request timestamp.</summary>
        /// <value>The server request timestamp.</value>
        public TimeSpan ServerRequestTimestamp { get; set; }

        /// <summary>Gets or sets the client request timestamp.</summary>
        /// <value>The client request timestamp.</value>
        public TimeSpan ClientRequestTimestamp { get; set; }

        /// <summary>Gets or sets the client response timestamp.</summary>
        /// <value>The client response timestamp.</value>
        public TimeSpan ClientResponseTimestamp { get; set; }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized instance.</returns>
        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(long) * 2))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ServerRequestTimestamp.Ticks);
                    bw.Write(this.ClientRequestTimestamp.Ticks);
                    bw.Write(this.ClientResponseTimestamp.Ticks);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
