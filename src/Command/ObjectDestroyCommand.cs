﻿//-----------------------------------------------------------------------
// <copyright file="ObjectDestroyCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;
    using Abune.Shared.DataType;

    /// <summary>
    /// Command class for object destruction.
    /// </summary>
    public class ObjectDestroyCommand : BaseCommand, ICanLocate, IObjectCommand
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectDestroyCommand" /> class.</summary>
        public ObjectDestroyCommand()
            : base(CommandType.ObjectDestroy)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectDestroyCommand" /> class.</summary>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Command argument.</exception>
        /// <exception cref="NotSupportedException">Type {command.Type} not supported.</exception>
        public ObjectDestroyCommand(BaseCommand command)
            : base(CommandType.ObjectDestroy)
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
                    this.FrameTick = br.ReadUInt64();
                    this.ObjectId = br.ReadUInt64();
                    this.TargetPosition = ReadVector3(br);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectDestroyCommand" /> class.</summary>
        /// <param name="frameTick">The frame tick.</param>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="targetPosition">The target position.</param>
        public ObjectDestroyCommand(ulong frameTick, ulong objectId, AVector3 targetPosition)
            : base(CommandType.ObjectDestroy)
        {
            this.FrameTick = frameTick;
            this.ObjectId = objectId;
            this.TargetPosition = targetPosition;
            int sizeUlongs = sizeof(ulong) * 2;
            int sizeFloats = sizeof(float) * 3;
            using (MemoryStream stream = new MemoryStream(sizeUlongs + sizeFloats))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.FrameTick);
                    bw.Write(this.ObjectId);
                    Write(bw, this.TargetPosition);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Gets or sets the frame tick.</summary>
        /// <value>The frame tick.</value>
        public ulong FrameTick { get; set; }

        /// <summary>Gets or sets the object identifier.</summary>
        /// <value>The object identifier.</value>
        public ulong ObjectId { get; set; }

        /// <summary>Gets or sets the target position x.</summary>
        /// <value>The target position x.</value>
        public AVector3 TargetPosition { get; set; }

        /// <summary>Gets the world position x axis.</summary>
        public AVector3 WorldPosition
        {
            get
            {
                return this.TargetPosition;
            }
        }
    }
}
