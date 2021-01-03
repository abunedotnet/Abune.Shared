//-----------------------------------------------------------------------
// <copyright file="UdpTransferFrame.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Protocol
{
    using System;
    using System.IO;
    using Abune.Shared.Message;
    using Abune.Shared.Message.Contract;

    /// <summary>Transfer frame udp frame.</summary>
    public class UdpTransferFrame
    {
        private const byte CONTROLBYTE = 0x66;
        private const uint MAXSIZE = 2000;

        /// <summary>Initializes a new instance of the <see cref="UdpTransferFrame" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public UdpTransferFrame(byte[] buffer)
        {
            this.Deserialize(buffer);
        }

        /// <summary>Initializes a new instance of the <see cref="UdpTransferFrame" /> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="messageBuffer">The message buffer.</param>
        /// <exception cref="ArgumentNullException">MessageBuffer is null.</exception>
        public UdpTransferFrame(FrameType type, byte[] messageBuffer)
        {
            if (messageBuffer == null)
            {
                throw new ArgumentNullException(nameof(messageBuffer));
            }

            this.Type = type;
            this.Length = (ushort)messageBuffer.Length;
            this.MessageBuffer = messageBuffer;
        }

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public FrameType Type { get; set; }

        /// <summary>Gets or sets the length.</summary>
        /// <value>The length.</value>
        public ushort Length { get; set; }

#pragma warning disable CA1819 // code efficiency
        /// <summary>Gets or sets the message buffer.</summary>
        /// <value>The message buffer.</value>
        public byte[] MessageBuffer { get; set; }

        /// <summary>Deserializes the specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="InternalBufferOverflowException">Invalid message size: {this.Length}.</exception>
        /// <exception cref="InvalidOperationException">Invalid control byte: {controlByte}.</exception>
        public void Deserialize(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.Type = (FrameType)br.ReadByte();
                    this.Length = br.ReadUInt16();
                    if (this.Length > MAXSIZE)
                    {
                        throw new InternalBufferOverflowException($"Invalid message size: {this.Length}");
                    }

                    this.MessageBuffer = br.ReadBytes((int)this.Length);
                    byte controlByte = br.ReadByte();
                    if (controlByte != CONTROLBYTE)
                    {
                        throw new InvalidOperationException($"Invalid control byte: {controlByte}");
                    }
                }

                stream.Flush();
            }
        }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized buffer.</returns>
        public virtual byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(byte) + sizeof(ushort) + this.MessageBuffer.Length))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write((byte)this.Type);
                    bw.Write((ushort)this.MessageBuffer.Length);
                    bw.Write(this.MessageBuffer, 0, this.MessageBuffer.Length);
                    bw.Write(CONTROLBYTE);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
