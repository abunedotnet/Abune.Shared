//-----------------------------------------------------------------------
// <copyright file="ClientHelloMessage.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System.IO;
    using System.Text;

    /// <summary>Client hello message.</summary>
    public class ClientHelloMessage
    {
        /// <summary>Initializes a new instance of the <see cref="ClientHelloMessage" /> class.</summary>
        public ClientHelloMessage()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ClientHelloMessage" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public ClientHelloMessage(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.ClientId = br.ReadUInt32();
                    this.ClientPort = br.ReadUInt32();
                    byte messageLength = br.ReadByte();
                    byte[] messageBytes = br.ReadBytes(messageLength);
                    this.Message = Encoding.UTF8.GetString(messageBytes);
                }
            }
        }

        /// <summary>
        ///  Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public uint ClientId { get; set; }

        /// <summary>Gets or sets the client udp port.</summary>
        /// <value>The client port.</value>
        public uint ClientPort { get; set; }

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized instance.</returns>
        public byte[] Serialize()
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(this.Message);
            using (MemoryStream stream = new MemoryStream(sizeof(uint) + messageBytes.Length))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ClientId);
                    bw.Write(this.ClientPort);
                    bw.Write((byte)messageBytes.Length);
                    bw.Write(messageBytes);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
