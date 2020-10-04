//-----------------------------------------------------------------------
// <copyright file="ObjectCollisionCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;

    /// <summary>
    /// Object collision command.
    /// </summary>
    public class ObjectCollisionCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCollisionCommand"/> class.
        /// </summary>
        public ObjectCollisionCommand()
            : base(CommandType.ObjectCollision)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCollisionCommand"/> class.
        /// </summary>
        /// <param name="command">Unextracted base command.</param>
        public ObjectCollisionCommand(BaseCommand command)
            : base(CommandType.ObjectCollision)
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
                    this.ObjectIdColliding = br.ReadUInt64();
                    this.FrameTick = br.ReadUInt64();
                    this.ImpulseX = br.ReadSingle();
                    this.ImpulseY = br.ReadSingle();
                    this.ImpulseZ = br.ReadSingle();
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCollisionCommand"/> class.
        /// </summary>
        /// <param name="objectIdColliding">Id of object colliding.</param>
        /// <param name="frameTick">Tick of the frame.</param>
        /// <param name="impulseX">Impulse on x axis in units per second.</param>
        /// <param name="impulseY">Impulse on y axis in units per second.</param>
        /// <param name="impulseZ">Impulse on z axis in units per second.</param>
        public ObjectCollisionCommand(
            ulong objectIdColliding,
            ulong frameTick,
            float impulseX,
            float impulseY,
            float impulseZ)
            : base(CommandType.ObjectCollision)
        {
            this.ObjectIdColliding = objectIdColliding;
            this.FrameTick = frameTick;
            this.ImpulseX = impulseX;
            this.ImpulseY = impulseY;
            this.ImpulseZ = impulseZ;
            using (MemoryStream stream = new MemoryStream(sizeof(ulong) + sizeof(uint) + sizeof(uint)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ObjectIdColliding);
                    bw.Write(this.FrameTick);
                    bw.Write(this.ImpulseX);
                    bw.Write(this.ImpulseY);
                    bw.Write(this.ImpulseZ);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the id of the colliding object.
        /// </summary>
        public ulong ObjectIdColliding { get; set; }

        /// <summary>
        /// Gets or sets the tick of the frame.
        /// </summary>
        public ulong FrameTick { get; set; }

        /// <summary>
        /// Gets or sets impulse x axis in units per second.
        /// </summary>
        public float ImpulseX { get; set; }

        /// <summary>
        /// Gets or sets impulse y axis in units per second.
        /// </summary>
        public float ImpulseY { get; set; }

        /// <summary>
        /// Gets or sets impulse z axis in units per second.
        /// </summary>
        public float ImpulseZ { get; set; }
    }
}
