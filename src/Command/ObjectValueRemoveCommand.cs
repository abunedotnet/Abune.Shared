//-----------------------------------------------------------------------
// <copyright file="ObjectValueRemoveCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;

    /// <summary>Remove value from object command.</summary>
    public class ObjectValueRemoveCommand : BaseCommand, IObjectCommand
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectValueRemoveCommand" /> class.</summary>
        public ObjectValueRemoveCommand()
            : base(CommandType.ObjectValueRemove)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectValueRemoveCommand" /> class.</summary>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Command is null.</exception>
        /// <exception cref="NotSupportedException">Type {command.Type} not supported.</exception>
        public ObjectValueRemoveCommand(BaseCommand command)
            : base(CommandType.ObjectValueRemove)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (command.Type != this.Type)
            {
                throw new NotSupportedException($"Type {command.Type} not supported.");
            }

            this.Body = command.Body;
            using (MemoryStream stream = new MemoryStream(this.Body))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.ObjectId = br.ReadUInt64();
                    this.ValueId = br.ReadUInt32();
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectValueRemoveCommand" /> class.</summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="valueId">The value identifier.</param>
        public ObjectValueRemoveCommand(ulong objectId, uint valueId)
            : base(CommandType.ObjectValueRemove)
        {
            this.ObjectId = objectId;
            this.ValueId = valueId;
            int size = sizeof(ulong) * 2;
            using (MemoryStream stream = new MemoryStream(size))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ObjectId);
                    bw.Write(this.ValueId);
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
    }
}
