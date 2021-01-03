//-----------------------------------------------------------------------
// <copyright file="ObjectUnlockCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;

    /// <summary>Unlock an object.</summary>
    public class ObjectUnlockCommand : BaseCommand, IObjectCommand
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectUnlockCommand" /> class.</summary>
        public ObjectUnlockCommand()
            : base(CommandType.ObjectUnlock)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectUnlockCommand" /> class.</summary>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Command is null.</exception>
        /// <exception cref="NotSupportedException">Type {command.Type} not supported.</exception>
        public ObjectUnlockCommand(BaseCommand command)
            : base(CommandType.ObjectUnlock)
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
                    this.LockOwnerId = br.ReadUInt32();
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectUnlockCommand" /> class.</summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="lockOwnerId">The lock owner identifier.</param>
        public ObjectUnlockCommand(ulong objectId, uint lockOwnerId)
            : base(CommandType.ObjectUnlock)
        {
            this.ObjectId = objectId;
            this.LockOwnerId = lockOwnerId;
            int sizeUlongs = sizeof(ulong) * 2;
            using (MemoryStream stream = new MemoryStream(sizeUlongs))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ObjectId);
                    bw.Write(lockOwnerId);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Gets or sets the object identifier.</summary>
        /// <value>The object identifier.</value>
        public ulong ObjectId { get; set; }

        /// <summary>Gets or sets the lock owner identifier.</summary>
        /// <value>The lock owner identifier.</value>
        public uint LockOwnerId { get; set; }
    }
}
