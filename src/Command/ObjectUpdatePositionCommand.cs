//-----------------------------------------------------------------------
// <copyright file="ObjectUpdatePositionCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;

    /// <summary>Update object position.</summary>
    public class ObjectUpdatePositionCommand : BaseCommand, ICanLocate, ICanRotate, ICanAccelerate
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

            this.Body = command.Body;
            using (MemoryStream stream = new MemoryStream(this.Body))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.TargetPositionX = br.ReadSingle();
                    this.TargetPositionY = br.ReadSingle();
                    this.TargetPositionZ = br.ReadSingle();
                    this.QuaternionW = br.ReadSingle();
                    this.QuaternionX = br.ReadSingle();
                    this.QuaternionY = br.ReadSingle();
                    this.QuaternionZ = br.ReadSingle();
                    this.VelocityX = br.ReadSingle();
                    this.VelocityY = br.ReadSingle();
                    this.VelocityZ = br.ReadSingle();
                    this.AngularVelocityX = br.ReadSingle();
                    this.AngularVelocityY = br.ReadSingle();
                    this.AngularVelocityZ = br.ReadSingle();
                    this.StartFrameTick = br.ReadUInt64();
                    this.StopFrameTick = br.ReadUInt64();
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectUpdatePositionCommand" /> class.</summary>
        /// <param name="targetPositionX">The target position x.</param>
        /// <param name="targetPositionY">The target position y.</param>
        /// <param name="targetPositionZ">The target position z.</param>
        /// <param name="quaternionW">The quaternion w.</param>
        /// <param name="quaternionX">The quaternion x.</param>
        /// <param name="quaternionY">The quaternion y.</param>
        /// <param name="quaternionZ">The quaternion z.</param>
        /// <param name="startFrameTick">The start frame tick.</param>
        /// <param name="stopFrameTick">The stop frame tick.</param>
        public ObjectUpdatePositionCommand(
            float targetPositionX,
            float targetPositionY,
            float targetPositionZ,
            float quaternionW,
            float quaternionX,
            float quaternionY,
            float quaternionZ,
            ulong startFrameTick,
            ulong stopFrameTick)
            : this(
            targetPositionX,
            targetPositionY,
            targetPositionZ,
            quaternionW,
            quaternionX,
            quaternionY,
            quaternionZ,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            startFrameTick,
            stopFrameTick)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectUpdatePositionCommand" /> class.</summary>
        /// <param name="targetPositionX">The target position x.</param>
        /// <param name="targetPositionY">The target position y.</param>
        /// <param name="targetPositionZ">The target position z.</param>
        /// <param name="quaternionW">The quaternion w.</param>
        /// <param name="quaternionX">The quaternion x.</param>
        /// <param name="quaternionY">The quaternion y.</param>
        /// <param name="quaternionZ">The quaternion z.</param>
        /// <param name="velocityX">The velocity x.</param>
        /// <param name="velocityY">The velocity y.</param>
        /// <param name="velocityZ">The velocity z.</param>
        /// <param name="angularVelocityX">The angular velocity x.</param>
        /// <param name="angularVelocityY">The angular velocity y.</param>
        /// <param name="angularVelocityZ">The angular velocity z.</param>
        /// <param name="startFrameTick">The start frame tick.</param>
        /// <param name="stopFrameTick">The stop frame tick.</param>
        public ObjectUpdatePositionCommand(
            float targetPositionX,
            float targetPositionY,
            float targetPositionZ,
            float quaternionW,
            float quaternionX,
            float quaternionY,
            float quaternionZ,
            float velocityX,
            float velocityY,
            float velocityZ,
            float angularVelocityX,
            float angularVelocityY,
            float angularVelocityZ,
            ulong startFrameTick,
            ulong stopFrameTick)
            : base(CommandType.ObjectUpdatePosition)
        {
            this.TargetPositionX = targetPositionX;
            this.TargetPositionY = targetPositionY;
            this.TargetPositionZ = targetPositionZ;
            this.QuaternionW = quaternionW;
            this.QuaternionX = quaternionX;
            this.QuaternionY = quaternionY;
            this.QuaternionZ = quaternionZ;
            this.VelocityX = velocityX;
            this.VelocityY = velocityY;
            this.VelocityZ = velocityZ;
            this.AngularVelocityX = angularVelocityX;
            this.AngularVelocityY = angularVelocityY;
            this.AngularVelocityZ = angularVelocityZ;
            this.StartFrameTick = startFrameTick;
            this.StopFrameTick = stopFrameTick;
            int sizeUlongs = sizeof(ulong) * 2;
            int sizeFloats = sizeof(float) * 13;
            int size = sizeUlongs + sizeFloats;
            using (MemoryStream stream = new MemoryStream(size))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.TargetPositionX);
                    bw.Write(this.TargetPositionY);
                    bw.Write(this.TargetPositionZ);
                    bw.Write(this.QuaternionW);
                    bw.Write(this.QuaternionX);
                    bw.Write(this.QuaternionY);
                    bw.Write(this.QuaternionZ);
                    bw.Write(this.VelocityX);
                    bw.Write(this.VelocityY);
                    bw.Write(this.VelocityZ);
                    bw.Write(this.AngularVelocityX);
                    bw.Write(this.AngularVelocityY);
                    bw.Write(this.AngularVelocityZ);
                    bw.Write(this.StartFrameTick);
                    bw.Write(this.StopFrameTick);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>Gets or sets the target position x.</summary>
        /// <value>The target position x.</value>
        public float TargetPositionX { get; set; }

        /// <summary>
        /// Gets or sets the target position y.
        /// </summary>
        /// <value>The target position y.</value>
        public float TargetPositionY { get; set; }

        /// <summary>Gets or sets the target position z.</summary>
        /// <value>The target position z.</value>
        public float TargetPositionZ { get; set; }

        /// <summary>Gets or sets the quaternion w.</summary>
        /// <value>The quaternion w.</value>
        public float QuaternionW { get; set; }

        /// <summary>Gets or sets the quaternion x.</summary>
        /// <value>The quaternion x.</value>
        public float QuaternionX { get; set; }

        /// <summary>Gets or sets the quaternion y.</summary>
        /// <value>The quaternion y.</value>
        public float QuaternionY { get; set; }

        /// <summary>Gets or sets the quaternion z.</summary>
        /// <value>The quaternion z.</value>
        public float QuaternionZ { get; set; }

        /// <summary>Gets or sets the velocity x.</summary>
        /// <value>The velocity x.</value>
        public float VelocityX { get; set; }

        /// <summary>Gets or sets the velocity y.</summary>
        /// <value>The velocity y.</value>
        public float VelocityY { get; set; }

        /// <summary>Gets or sets the velocity z.</summary>
        /// <value>The velocity z.</value>
        public float VelocityZ { get; set; }

        /// <summary>Gets or sets the angular velocity x.</summary>
        /// <value>The angular velocity x.</value>
        public float AngularVelocityX { get; set; }

        /// <summary>Gets or sets the angular velocity y.</summary>
        /// <value>The angular velocity y.</value>
        public float AngularVelocityY { get; set; }

        /// <summary>Gets or sets the angular velocity z.</summary>
        /// <value>The angular velocity z.</value>
        public float AngularVelocityZ { get; set; }

        /// <summary>Gets or sets the start frame tick.</summary>
        /// <value>The start frame tick.</value>
        public ulong StartFrameTick { get; set; }

        /// <summary>Gets or sets the stop frame tick.</summary>
        /// <value>The stop frame tick.</value>
        public ulong StopFrameTick { get; set; }

#pragma warning disable CA1033 // convert meaning
        /// <summary>Gets the world position x axis.</summary>
        float ICanLocate.WorldPositionX
        {
            get
            {
                return this.TargetPositionX;
            }
        }

        /// <summary>Gets the world position y axis.</summary>
        float ICanLocate.WorldPositionY
        {
            get
            {
                return this.TargetPositionY;
            }
        }

        /// <summary>
        /// Gets the world position z axis.
        /// </summary>
        float ICanLocate.WorldPositionZ
        {
            get
            {
                return this.TargetPositionZ;
            }
        }

        /// <summary>Gets the rotation w.</summary>
        /// <value>The rotation w.</value>
        float ICanRotate.RotationW
        {
            get
            {
                return this.QuaternionW;
            }
        }

        /// <summary>Gets the rotation x.</summary>
        /// <value>The rotation x.</value>
        float ICanRotate.RotationX
        {
            get
            {
                return this.QuaternionX;
            }
        }

        /// <summary>Gets the rotation y.</summary>
        /// <value>The rotation y.</value>
        float ICanRotate.RotationY
        {
            get
            {
                return this.QuaternionY;
            }
        }

        /// <summary>Gets the rotation z.</summary>
        /// <value>The rotation z.</value>
        float ICanRotate.RotationZ
        {
            get
            {
                return this.QuaternionZ;
            }
        }

        /// <summary>Gets the velocity on x axis in units per second.</summary>
        float ICanAccelerate.VelocityX
        {
            get
            {
                return this.VelocityX;
            }
        }

        /// <summary>Gets the velocity on y axis in units per second.</summary>
        float ICanAccelerate.VelocityY
        {
            get
            {
                return this.VelocityY;
            }
        }

        /// <summary>Gets the velocity on z axis in units per second.</summary>
        float ICanAccelerate.VelocityZ
        {
            get
            {
                return this.VelocityZ;
            }
        }

        /// <summary>Gets the angular velocity on x axis in radiants per second.</summary>
        float ICanAccelerate.AngularVelocityX
        {
            get
            {
                return this.AngularVelocityX;
            }
        }

        /// <summary>Gets the angular velocity on x axis in radiants per second.</summary>
        float ICanAccelerate.AngularVelocityY
        {
            get
            {
                return this.AngularVelocityY;
            }
        }

        /// <summary>Gets the angular velocity on x axis in radiants per second.</summary>
        float ICanAccelerate.AngularVelocityZ
        {
            get
            {
                return this.AngularVelocityZ;
            }
        }
    }
}
