//-----------------------------------------------------------------------
// <copyright file="BaseCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716

namespace Abune.Shared.Command
{
    using System;
    using System.IO;

    /// <summary>
    /// Base class for all commands.
    /// </summary>
    public class BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        /// <param name="buffer">Buffer containing the serialized command.</param>
        public BaseCommand(byte[] buffer)
        {
            this.Deserialize(buffer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        /// <param name="br">Binary reader on the buffer containing payload.</param>
        public BaseCommand(BinaryReader br)
        {
            this.Deserialize(br);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        /// <param name="type">Type of command.</param>
        public BaseCommand(CommandType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        /// <param name="priority">Command priority.</param>
        /// <param name="type">Command type.</param>
        /// <param name="commandBody">Body payload of command.</param>
        public BaseCommand(byte priority, CommandType type, byte[] commandBody)
        {
            this.Priority = priority;
            this.Type = type;
            this.Body = commandBody;
        }

        /// <summary>
        /// Gets the command priority.
        /// </summary>
        public byte Priority { get; private set; }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public CommandType Type { get; private set; }

        /// <summary>
        /// Gets or sets the command payload.
        /// </summary>
        #pragma warning disable CA1819 // code efficiency
        public byte[] Body { get; protected set; }

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns>Serialized instance.</returns>
        public virtual byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(ulong) + sizeof(byte) + sizeof(ulong) + sizeof(byte) + this.Body.Length))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    this.Serialize(bw);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Serializes this instance into a binary writer.
        /// </summary>
        /// <param name="bw">Binary write to serialize to.</param>
        public virtual void Serialize(BinaryWriter bw)
        {
            if (bw == null)
            {
                throw new ArgumentNullException(nameof(bw));
            }

            bw.Write((byte)this.Priority);
            bw.Write((uint)this.Type);
            bw.Write((uint)this.Body.Length);
            bw.Write(this.Body);
        }

        /// <summary>
        /// Deserializes the buffer into this instance.
        /// </summary>
        /// <param name="buffer">Byte buffer.</param>
        protected void Deserialize(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.Deserialize(br);
                }

                stream.Flush();
            }
        }

        /// <summary>
        /// Deserializes the binary reader into this instance.
        /// </summary>
        /// <param name="br">Binary reader on byte buffer.</param>
        protected void Deserialize(BinaryReader br)
        {
            if (br == null)
            {
                throw new ArgumentNullException(nameof(br));
            }

            this.Priority = br.ReadByte();
            this.Type = (CommandType)br.ReadUInt32();
            uint bodylength = br.ReadUInt32();
            this.Body = br.ReadBytes((int)bodylength);
        }
    }
}
