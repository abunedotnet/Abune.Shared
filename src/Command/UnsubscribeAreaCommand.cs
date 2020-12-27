//-----------------------------------------------------------------------
// <copyright file="UnsubscribeAreaCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;

    /// <summary>Unsubscribe client from area command.</summary>
    public class UnsubscribeAreaCommand : BaseCommand, IAreaCommand
    {
        /// <summary>Initializes a new instance of the <see cref="UnsubscribeAreaCommand" /> class.</summary>
        public UnsubscribeAreaCommand()
            : base(CommandType.UnsubscribeArea)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="UnsubscribeAreaCommand" /> class.</summary>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Command is null.</exception>
        /// <exception cref="NotSupportedException">Type {command.Type} not supported.</exception>
        public UnsubscribeAreaCommand(BaseCommand command)
            : base(CommandType.UnsubscribeArea)
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
                    this.ClientId = br.ReadUInt32();
                    this.AreaId = br.ReadUInt64();
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="UnsubscribeAreaCommand" /> class.</summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="areaId">The area identifier.</param>
        public UnsubscribeAreaCommand(uint clientId, ulong areaId)
            : base(CommandType.UnsubscribeArea)
        {
            this.ClientId = clientId;
            this.AreaId = areaId;
            using (MemoryStream stream = new MemoryStream(sizeof(uint) + sizeof(ulong)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
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
    }
}
