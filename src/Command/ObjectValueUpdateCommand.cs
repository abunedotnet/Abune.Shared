//-----------------------------------------------------------------------
// <copyright file="ObjectValueUpdateCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;

    /// <summary>Update value of object command.</summary>
    public class ObjectValueUpdateCommand : BaseCommand, IObjectCommand
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectValueUpdateCommand" /> class.</summary>
        public ObjectValueUpdateCommand()
            : base(CommandType.ObjectValueUpdate)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectValueUpdateCommand" /> class.</summary>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Command is null.</exception>
        /// <exception cref="NotSupportedException">Type {command.Type} not supported.</exception>
        public ObjectValueUpdateCommand(BaseCommand command)
            : base(CommandType.ObjectValueUpdate)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (command.Type != this.Type)
            {
                throw new NotSupportedException($"Type {command.Type} not supported.");
            }

            this.CopyFrom(command);
            using (MemoryStream stream = new MemoryStream(this.Body))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.ObjectId = br.ReadUInt64();
                    this.ValueId = br.ReadUInt32();
                    int length = br.ReadInt32();
                    this.Data = br.ReadBytes(length);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectValueUpdateCommand" /> class.</summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="valueId">The value identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="commandFlags">The command flags.</param>
        /// <param name="quorumHash">The quorum hash.</param>
        /// <exception cref="ArgumentNullException">Data is null.</exception>
        public ObjectValueUpdateCommand(ulong objectId, uint valueId, byte[] data, CommandFlags commandFlags = CommandFlags.None, ulong quorumHash = 0)
            : base(CommandType.ObjectValueUpdate, flags: commandFlags, quorumHash: quorumHash)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.ObjectId = objectId;
            this.ValueId = valueId;
            this.Data = data;
            int sizeULongs = sizeof(ulong) * 3;
            using (MemoryStream stream = new MemoryStream(sizeULongs + data.Length))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ObjectId);
                    bw.Write(this.ValueId);
                    bw.Write(this.Data.Length);
                    bw.Write(this.Data);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Gets or sets the object identifier.</summary>
        /// <value>The object identifier.</value>
        public ulong ObjectId { get; set; }

        /// <summary>Gets or sets the value identifier.</summary>
        /// <value>The value identifier.</value>
        public uint ValueId { get; set; }

#pragma warning disable CA1819 // code efficiency
        /// <summary>Gets or sets the data.</summary>
        /// <value>The data.</value>
        public byte[] Data { get; set; }
    }
}
