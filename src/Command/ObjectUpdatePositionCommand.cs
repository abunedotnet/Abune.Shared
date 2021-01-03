//-----------------------------------------------------------------------
// <copyright file="ObjectUpdatePositionCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;
    using Abune.Shared.DataType;

    /// <summary>Update object position.</summary>
    public class ObjectUpdatePositionCommand : BaseCommand, ICanLocate, ICanRotate, ICanAccelerate, IObjectCommand
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectUpdatePositionCommand" /> class.</summary>
        public ObjectUpdatePositionCommand()
            : base(CommandType.ObjectUpdatePosition)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectUpdatePositionCommand" /> class.</summary>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Command is null.</exception>
        /// <exception cref="NotSupportedException">Type {command.Type} not supported.</exception>
        public ObjectUpdatePositionCommand(BaseCommand command)
            : base(CommandType.ObjectUpdatePosition)
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
                    this.TargetPosition = ReadVector3(br);
                    this.TargetOrientation = ReadQuaternion(br);
                    this.Velocity = ReadVector3(br);
                    this.AngularVelocity = ReadVector3(br);
                    this.StartFrameTick = br.ReadUInt64();
                    this.StopFrameTick = br.ReadUInt64();
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectUpdatePositionCommand" /> class.</summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="targetPosition">The target position x.</param>
        /// <param name="targetOrientation">The quaternion .</param>
        /// <param name="startFrameTick">The start frame tick.</param>
        /// <param name="stopFrameTick">The stop frame tick.</param>
        public ObjectUpdatePositionCommand(
            ulong objectId,
            AVector3 targetPosition,
            AQuaternion targetOrientation,
            ulong startFrameTick,
            ulong stopFrameTick)
            : this(
                objectId,
                targetPosition,
                targetOrientation,
                AVector3.Zero,
                AVector3.Zero,
                startFrameTick,
                stopFrameTick)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectUpdatePositionCommand" /> class.</summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="targetPosition">The target position.</param>
        /// <param name="targetOrientation">The quaternion.</param>
        /// <param name="velocity">The velocity.</param>
        /// <param name="angularVelocity">The angular velocity.</param>
        /// <param name="startFrameTick">The start frame tick.</param>
        /// <param name="stopFrameTick">The stop frame tick.</param>
        public ObjectUpdatePositionCommand(
            ulong objectId,
            AVector3 targetPosition,
            AQuaternion targetOrientation,
            AVector3 velocity,
            AVector3 angularVelocity,
            ulong startFrameTick,
            ulong stopFrameTick)
            : base(CommandType.ObjectUpdatePosition)
        {
            this.ObjectId = objectId;
            this.TargetPosition = targetPosition;
            this.TargetOrientation = targetOrientation;
            this.Velocity = velocity;
            this.AngularVelocity = angularVelocity;
            this.StartFrameTick = startFrameTick;
            this.StopFrameTick = stopFrameTick;
            int sizeUlongs = sizeof(ulong) * 2;
            int sizeFloats = sizeof(float) * 13;
            int size = sizeUlongs + sizeFloats;
            using (MemoryStream stream = new MemoryStream(size))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.ObjectId);
                    Write(bw, this.TargetPosition);
                    Write(bw, this.TargetOrientation);
                    Write(bw, this.Velocity);
                    Write(bw, this.AngularVelocity);
                    bw.Write(this.StartFrameTick);
                    bw.Write(this.StopFrameTick);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Gets or sets the object identifier.</summary>
        /// <value>The object identifier.</value>
        public ulong ObjectId { get; set; }

        /// <summary>Gets or sets the target position.</summary>
        /// <value>The target position.</value>
        public AVector3 TargetPosition { get; set; }

        /// <summary>Gets or sets the quaternion.</summary>
        /// <value>The quaternion.</value>
        public AQuaternion TargetOrientation { get; set; }

        /// <summary>Gets or sets the velocity.</summary>
        /// <value>The velocity.</value>
        public AVector3 Velocity { get; set; }

        /// <summary>Gets or sets the angular velocity.</summary>
        /// <value>The angular velocity.</value>
        public AVector3 AngularVelocity { get; set; }

        /// <summary>Gets or sets the start frame tick.</summary>
        /// <value>The start frame tick.</value>
        public ulong StartFrameTick { get; set; }

        /// <summary>Gets or sets the stop frame tick.</summary>
        /// <value>The stop frame tick.</value>
        public ulong StopFrameTick { get; set; }

#pragma warning disable CA1033 // convert meaning
        /// <summary>Gets the world position.</summary>
        AVector3 ICanLocate.WorldPosition
        {
            get
            {
                return this.TargetPosition;
            }
        }

        /// <summary>Gets the rotation.</summary>
        /// <value>The rotation.</value>
        AQuaternion ICanRotate.Orientation
        {
            get
            {
                return this.TargetOrientation;
            }
        }

        /// <summary>Gets the velocity in units per second.</summary>
        AVector3 ICanAccelerate.Velocity
        {
            get
            {
                return this.Velocity;
            }
        }

        /// <summary>Gets the angular velocity radiants per second.</summary>
        AVector3 ICanAccelerate.AngularVelocity
        {
            get
            {
                return this.AngularVelocity;
            }
        }
    }
}
