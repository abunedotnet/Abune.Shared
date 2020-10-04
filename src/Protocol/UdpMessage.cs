//-----------------------------------------------------------------------
// <copyright file="UdpMessage.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Protocol
{
    using System;
    using System.IO;

    /// <summary>Udp layer message transporting frames and command messages.</summary>
    public class UdpMessage
    {
        /// <summary>Initializes a new instance of the <see cref="UdpMessage" /> class.</summary>
        /// <param name="sequenceId">The sequence identifier.</param>
        /// <param name="controlFlags">The control flags.</param>
        /// <param name="commandBody">The command body.</param>
        public UdpMessage(uint sequenceId, MessageControlFlags controlFlags, byte[] commandBody)
        {
            this.SequenceId = sequenceId;
            this.ControlFlags = controlFlags;
            this.CommandBody = commandBody;
        }

        /// <summary>Initializes a new instance of the <see cref="UdpMessage" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public UdpMessage(byte[] buffer)
        {
            this.Deserialize(buffer);
        }

        /// <summary>Initializes a new instance of the <see cref="UdpMessage" /> class.</summary>
        /// <param name="br">The br.</param>
        public UdpMessage(BinaryReader br)
        {
            this.Deserialize(br);
        }

        /// <summary>Gets or sets the sequence identifier.</summary>
        /// <value>The sequence identifier.</value>
        public uint SequenceId { get; set; }

        /// <summary>Gets or sets the control flags.</summary>
        /// <value>The control flags.</value>
        public MessageControlFlags ControlFlags { get; set; }

#pragma warning disable CA1819 // code efficiency
        /// <summary>Gets or sets the command body.</summary>
        /// <value>The command body.</value>
        public byte[] CommandBody { get; set; }

        /// <summary>Deserializes the specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        public void Deserialize(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.Deserialize(br);
                }
            }
        }

        /// <summary>Deserializes the specified br.</summary>
        /// <param name="br">The br.</param>
        /// <exception cref="ArgumentNullException">Binary reader is null.</exception>
        public void Deserialize(BinaryReader br)
        {
            if (br == null)
            {
                throw new ArgumentNullException(nameof(br));
            }

            this.SequenceId = br.ReadUInt32();
            this.ControlFlags = (MessageControlFlags)br.ReadByte();
            if ((this.ControlFlags & MessageControlFlags.COMMAND) == MessageControlFlags.COMMAND)
            {
                uint bodylength = br.ReadUInt32();
                this.CommandBody = br.ReadBytes((int)bodylength);
            }
        }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized udp message.</returns>
        public virtual byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(ulong) + sizeof(byte) + sizeof(ulong) + sizeof(byte) + this.CommandBody.Length))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write((uint)this.SequenceId);
                    bw.Write((byte)this.ControlFlags);
                    if ((this.ControlFlags & MessageControlFlags.ACK) == MessageControlFlags.ACK /*ACK, ACK2*/ || (this.ControlFlags & MessageControlFlags.NACK) == MessageControlFlags.NACK /*NACK, REJ*/)
                    {
                        // bw.Write(ResponseMessageId);
                    }

                    if ((this.ControlFlags & MessageControlFlags.COMMAND) == MessageControlFlags.COMMAND)
                    {
                        bw.Write((uint)this.CommandBody.Length);
                        bw.Write(this.CommandBody);
                    }
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
