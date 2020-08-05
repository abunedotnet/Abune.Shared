//-----------------------------------------------------------------------
// <copyright file="ClientPingMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System;
    using System.IO;

    /// <summary>Client ping request message.</summary>
    public class ClientPingMessage
    {
        /// <summary>Initializes a new instance of the <see cref="ClientPingMessage" /> class.</summary>
        public ClientPingMessage()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ClientPingMessage" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public ClientPingMessage(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.ClientTimestamp = TimeSpan.FromTicks(br.ReadInt64());
                }
            }
        }

        /// <summary>Gets or sets the client timestamp.</summary>
        /// <value>The client timestamp.</value>
        public TimeSpan ClientTimestamp { get; set; }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized instance.</returns>
        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(long)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ClientTimestamp.Ticks);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
