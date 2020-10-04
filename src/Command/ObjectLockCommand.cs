//-----------------------------------------------------------------------
// <copyright file="ObjectLockCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;

    /// <summary>Command to lock objects.</summary>
    public class ObjectLockCommand : BaseCommand
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectLockCommand" /> class.</summary>
        public ObjectLockCommand()
            : base(CommandType.ObjectLock)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectLockCommand" /> class.</summary>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Command is null.</exception>
        /// <exception cref="NotSupportedException">Type {command.Type} not supported.</exception>
        public ObjectLockCommand(BaseCommand command)
            : base(CommandType.ObjectLock)
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
                    this.LockOwnerId = br.ReadUInt32();
                    this.Timeout = TimeSpan.FromTicks(br.ReadInt64());
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectLockCommand" /> class.</summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="lockOwnerId">The lock owner identifier.</param>
        /// <param name="timeout">The timeout.</param>
        public ObjectLockCommand(ulong objectId, uint lockOwnerId, TimeSpan timeout)
            : base(CommandType.ObjectLock)
        {
            this.ObjectId = objectId;
            this.LockOwnerId = lockOwnerId;
            this.Timeout = timeout;

            using (MemoryStream stream = new MemoryStream(sizeof(ulong) + sizeof(uint) + sizeof(uint)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ObjectId);
                    bw.Write(lockOwnerId);
                    bw.Write(this.Timeout.Ticks);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>The object identifier.</value>
        public ulong ObjectId { get; set; }

        /// <summary>Gets or sets the lock owner identifier.</summary>
        /// <value>The lock owner identifier.</value>
        public uint LockOwnerId { get; set; }

        /// <summary>Gets or sets the timeout.</summary>
        /// <value>The timeout.</value>
        public TimeSpan Timeout { get; set; }
    }
}
