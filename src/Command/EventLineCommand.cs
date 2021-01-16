//-----------------------------------------------------------------------
// <copyright file="EventLineCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;
    using Abune.Shared.DataType;

    /// <summary>Linear event command.</summary>
    /// <remarks>F.e. used for shooting events.</remarks>
    public class EventLineCommand : BaseCommand, ICanLocate
    {
        /// <summary>Initializes a new instance of the <see cref="EventLineCommand" /> class.</summary>
        public EventLineCommand()
            : base(CommandType.EventLine)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EventLineCommand" /> class.</summary>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Command is null.</exception>
        /// <exception cref="NotSupportedException">Type {command.Type} not supported.</exception>
        public EventLineCommand(BaseCommand command)
            : base(CommandType.EventLine)
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
                    this.EventType = br.ReadUInt16();
                    this.EventId = br.ReadUInt64();
                    this.StartPosition = ReadVector3(br);
                    this.EndPosition = ReadVector3(br);
                    this.FrameTick = br.ReadUInt64();
                    int length = br.ReadInt32();
                    this.Data = br.ReadBytes(length);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="EventLineCommand" /> class.</summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="startPosition">The start position.</param>
        /// <param name="endPosition">The end position.</param>
        /// <param name="frameTick">The frame tick.</param>
        /// <param name="data">The data.</param>
        /// <exception cref="ArgumentNullException">Data is null.</exception>
        public EventLineCommand(ushort eventType, ulong eventId, AVector3 startPosition, AVector3 endPosition, ulong frameTick, byte[] data)
            : base(CommandType.EventLine)
        {
            this.EventType = eventType;
            this.EventId = eventId;
            this.StartPosition = startPosition;
            this.EndPosition = endPosition;
            this.FrameTick = frameTick;
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
            using (MemoryStream stream = new MemoryStream(sizeof(ushort) + data.Length))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.EventType);
                    bw.Write(this.EventId);
                    Write(bw, this.StartPosition);
                    Write(bw, this.EndPosition);
                    bw.Write(this.FrameTick);
                    bw.Write(this.Data.Length);
                    bw.Write(this.Data);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Gets or sets the event type.</summary>
        /// <value>The object identifier.</value>
        public ushort EventType { get; set; }

        /// <summary>Gets or sets the event id.</summary>
        /// <value>The event id.</value>
        public ulong EventId { get; set; }

        /// <summary>Gets or sets the frame tick.</summary>
        /// <value>The frame tick when the event occured/will occur.</value>
        public ulong FrameTick { get; set; }

        /// <summary>
        /// Gets or sets position where the event starts.
        /// </summary>
        public AVector3 StartPosition { get; set; }

        /// <summary>
        /// Gets or sets position where the event ends.
        /// </summary>
        public AVector3 EndPosition { get; set; }

#pragma warning disable CA1819 // code efficiency
        /// <summary>Gets or sets the data.</summary>
        /// <value>The data.</value>
        public byte[] Data { get; set; }

#pragma warning disable CA1033 // convert meaning

        /// <summary>
        /// Gets position where object is located.
        /// </summary>
        AVector3 ICanLocate.WorldPosition
        {
            get
            {
                return this.StartPosition;
            }
        }
    }
}
