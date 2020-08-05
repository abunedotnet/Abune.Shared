//-----------------------------------------------------------------------
// <copyright file="ServerHelloMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System.IO;
    using System.Text;

    /// <summary>Server welcome message.</summary>
    public class ServerHelloMessage
    {
        /// <summary>Initializes a new instance of the <see cref="ServerHelloMessage" /> class.</summary>
        public ServerHelloMessage()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ServerHelloMessage" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public ServerHelloMessage(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    byte length = br.ReadByte();
                    byte[] messagePayload = br.ReadBytes(length);
                    this.Message = Encoding.UTF8.GetString(messagePayload);
                }
            }
        }

        /// <summary>Gets or sets the welcome message.</summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized instance.</returns>
        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(long)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    byte[] messagePayload = Encoding.UTF8.GetBytes(this.Message);
                    bw.Write((byte)messagePayload.Length);
                    bw.Write(messagePayload);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
