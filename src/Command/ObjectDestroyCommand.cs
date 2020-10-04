//-----------------------------------------------------------------------
// <copyright file="ObjectDestroyCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;

    /// <summary>
    /// Command class for object destruction.
    /// </summary>
    public class ObjectDestroyCommand : BaseCommand, ICanLocate
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

            this.Body = command.Body;
            using (MemoryStream stream = new MemoryStream(this.Body))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.FrameTick = br.ReadUInt64();
                    this.ObjectId = br.ReadUInt64();
                    this.TargetPositionX = br.ReadSingle();
                    this.TargetPositionY = br.ReadSingle();
                    this.TargetPositionZ = br.ReadSingle();
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectDestroyCommand" /> class.</summary>
        /// <param name="frameTick">The frame tick.</param>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="targetPositionX">The target position x.</param>
        /// <param name="targetPositionY">The target position y.</param>
        /// <param name="targetPositionZ">The target position z.</param>
        public ObjectDestroyCommand(ulong frameTick, ulong objectId, float targetPositionX, float targetPositionY, float targetPositionZ)
            : base(CommandType.ObjectDestroy)
        {
            this.FrameTick = frameTick;
            this.ObjectId = objectId;
            this.TargetPositionX = targetPositionX;
            this.TargetPositionY = targetPositionY;
            this.TargetPositionZ = targetPositionZ;
            int sizeUlongs = sizeof(ulong) * 2;
            int sizeFloats = sizeof(float) * 3;
            using (MemoryStream stream = new MemoryStream(sizeUlongs + sizeFloats))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.FrameTick);
                    bw.Write(this.ObjectId);
                    bw.Write(this.TargetPositionX);
                    bw.Write(this.TargetPositionY);
                    bw.Write(this.TargetPositionZ);
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
        public float TargetPositionX { get; set; }

        /// <summary>Gets or sets the target position y.</summary>
        /// <value>The target position y.</value>
        public float TargetPositionY { get; set; }

        /// <summary>Gets or sets the target position z.</summary>
        /// <value>The target position z.</value>
        public float TargetPositionZ { get; set; }

        /// <summary>Gets the world position x axis.</summary>
        public float WorldPositionX
        {
            get
            {
                return this.TargetPositionX;
            }
        }

        /// <summary>Gets the world position y axis.</summary>
        public float WorldPositionY
        {
            get
            {
                return this.TargetPositionY;
            }
        }

        /// <summary>Gets the world position z axis.</summary>
        public float WorldPositionZ
        {
            get
            {
                return this.TargetPositionZ;
            }
        }
    }
}
