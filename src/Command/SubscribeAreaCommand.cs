//-----------------------------------------------------------------------
// <copyright file="SubscribeAreaCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;

    /// <summary>Subscribe client to area command.</summary>
    public class SubscribeAreaCommand : BaseCommand
    {
        /// <summary>Initializes a new instance of the <see cref="SubscribeAreaCommand" /> class.</summary>
        public SubscribeAreaCommand()
            : base(CommandType.SubscribeArea)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SubscribeAreaCommand" /> class.</summary>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Command is null.</exception>
        /// <exception cref="NotSupportedException">Type {command.Type} not supported.</exception>
        public SubscribeAreaCommand(BaseCommand command)
            : base(CommandType.SubscribeArea)
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
                    this.MessagePriority = br.ReadUInt16();
                    this.ClientId = br.ReadUInt32();
                    this.AreaId = br.ReadUInt64();
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="SubscribeAreaCommand" /> class.</summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="areaId">The area identifier.</param>
        /// <param name="messagePriority">The message priority.</param>
        public SubscribeAreaCommand(uint clientId, ulong areaId, ushort messagePriority)
            : base(CommandType.SubscribeArea)
        {
            this.ClientId = clientId;
            this.AreaId = areaId;
            using (MemoryStream stream = new MemoryStream(sizeof(uint) + sizeof(ulong)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(messagePriority);
                    bw.Write(clientId);
                    bw.Write(areaId);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Gets the client identifier.</summary>
        /// <value>The client identifier.</value>
        public uint ClientId { get; private set; }

        /// <summary>Gets the area identifier.</summary>
        /// <value>The area identifier.</value>
        public ulong AreaId { get; private set; }

        /// <summary>Gets the message priority.</summary>
        /// <value>The message priority.</value>
        public ushort MessagePriority { get; private set; }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"Client:{this.ClientId} => Area:{this.AreaId}";
        }
    }
}
