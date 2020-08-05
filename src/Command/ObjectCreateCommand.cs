//-----------------------------------------------------------------------
// <copyright file="ObjectCreateCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;
    using System.IO;
    using Abune.Shared.Command.Contract;

    /// <summary>
    /// Object create command.
    /// </summary>
    public class ObjectCreateCommand : BaseCommand, ICanLocate, ICanRotate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCreateCommand"/> class.
        /// </summary>
        public ObjectCreateCommand()
            : base(CommandType.ObjectCreate)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCreateCommand"/> class.
        /// </summary>
        /// <param name="command">Unextracted base command.</param>
        public ObjectCreateCommand(BaseCommand command)
            : base(CommandType.ObjectCreate)
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
                    this.ParentObjectId = br.ReadUInt64();
                    this.OwnerId = br.ReadUInt32();
                    this.TypeId = br.ReadUInt32();
                    this.TargetPositionX = br.ReadSingle();
                    this.TargetPositionY = br.ReadSingle();
                    this.TargetPositionZ = br.ReadSingle();
                    this.QuaternionW = br.ReadSingle();
                    this.QuaternionX = br.ReadSingle();
                    this.QuaternionY = br.ReadSingle();
                    this.QuaternionZ = br.ReadSingle();
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCreateCommand"/> class.
        /// </summary>
        /// <param name="frameTick">Frame tick when the object was created.</param>
        /// <param name="objectId">Id of the new object.</param>
        /// <param name="parentObjectId">Id of the objects parent.</param>
        /// <param name="ownerId">Id of the owner.</param>
        /// <param name="typeId">Type of the object.</param>
        /// <param name="targetPositionX">Position x where object is located.</param>
        /// <param name="targetPositionY">Position y where object is located.</param>
        /// <param name="targetPositionZ">Position z where object is located.</param>
        /// <param name="quaternionW">Quaternion direction w axis of object.</param>
        /// <param name="quaternionX">Quaternion direction x axis of object.</param>
        /// <param name="quaternionY">Quaternion direction y axis of object.</param>
        /// <param name="quaternionZ">Quaternion direction z axis of object.</param>
        public ObjectCreateCommand(
            ulong frameTick,
            ulong objectId,
            ulong parentObjectId,
            uint ownerId,
            uint typeId,
            float targetPositionX,
            float targetPositionY,
            float targetPositionZ,
            float quaternionW,
            float quaternionX,
            float quaternionY,
            float quaternionZ)
            : base(CommandType.ObjectCreate)
        {
            this.FrameTick = frameTick;
            this.ObjectId = objectId;
            this.ParentObjectId = parentObjectId;
            this.OwnerId = ownerId;
            this.TypeId = typeId;
            this.TargetPositionX = targetPositionX;
            this.TargetPositionY = targetPositionY;
            this.TargetPositionZ = targetPositionZ;
            this.QuaternionW = quaternionW;
            this.QuaternionX = quaternionX;
            this.QuaternionY = quaternionY;
            this.QuaternionZ = quaternionZ;
            int floatSize = sizeof(float) * 7;
            using (MemoryStream stream = new MemoryStream(sizeof(ulong) + floatSize))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.FrameTick);
                    bw.Write(this.ObjectId);
                    bw.Write(this.ParentObjectId);
                    bw.Write(this.OwnerId);
                    bw.Write(this.TypeId);
                    bw.Write(this.TargetPositionX);
                    bw.Write(this.TargetPositionY);
                    bw.Write(this.TargetPositionZ);
                    bw.Write(this.QuaternionW);
                    bw.Write(this.QuaternionX);
                    bw.Write(this.QuaternionY);
                    bw.Write(this.QuaternionZ);
                }

                stream.Flush();
                this.Body = stream.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets frame tick when the object was created.
        /// </summary>
        public ulong FrameTick { get; set; }

        /// <summary>
        /// Gets or sets id of the new object.
        /// </summary>
        public ulong ObjectId { get; set; }

        /// <summary>
        /// Gets or sets id of the objects parent.
        /// </summary>
        public ulong ParentObjectId { get; set; }

        /// <summary>
        /// Gets or sets id of the owner.
        /// </summary>
        public uint OwnerId { get; set; }

        /// <summary>
        /// Gets or sets type of the object.
        /// </summary>
        public uint TypeId { get; set; }

        /// <summary>
        /// Gets or sets position x where object is located.
        /// </summary>
        public float TargetPositionX { get; set; }

        /// <summary>
        /// Gets or sets position y where object is located.
        /// </summary>
        public float TargetPositionY { get; set; }

        /// <summary>
        /// Gets or sets position z where object is located.
        /// </summary>
        public float TargetPositionZ { get; set; }

        /// <summary>
        /// Gets or sets quaternion direction w axis of object.
        /// </summary>
        public float QuaternionW { get; set; }

        /// <summary>
        /// Gets or sets quaternion direction x axis of object.
        /// </summary>
        public float QuaternionX { get; set; }

        /// <summary>
        /// Gets or sets quaternion direction y axis of object.
        /// </summary>
        public float QuaternionY { get; set; }

        /// <summary>
        /// Gets or sets quaternion direction z axis of object.
        /// </summary>
        public float QuaternionZ { get; set; }

#pragma warning disable CA1033 // convert meaning

        /// <summary>
        /// Gets position x where object is located.
        /// </summary>
        float ICanLocate.WorldPositionX
        {
            get
            {
                return this.TargetPositionX;
            }
        }

        /// <summary>
        /// Gets position x where object is located.
        /// </summary>
        float ICanLocate.WorldPositionY
        {
            get
            {
                return this.TargetPositionY;
            }
        }

        /// <summary>
        /// Gets position x where object is located.
        /// </summary>
        float ICanLocate.WorldPositionZ
        {
            get
            {
                return this.TargetPositionZ;
            }
        }

        /// <summary>
        /// Gets quaternion direction w axis of object.
        /// </summary>
        float ICanRotate.RotationW
        {
            get
            {
                return this.QuaternionW;
            }
        }

        /// <summary>
        /// Gets quaternion direction x axis of object.
        /// </summary>
        float ICanRotate.RotationX
        {
            get
            {
                return this.QuaternionX;
            }
        }

        /// <summary>
        /// Gets quaternion direction y axis of object.
        /// </summary>
        float ICanRotate.RotationY
        {
            get
            {
                return this.QuaternionY;
            }
        }

        /// <summary>
        /// Gets quaternion direction z axis of object.
        /// </summary>
        float ICanRotate.RotationZ
        {
            get
            {
                return this.QuaternionZ;
            }
        }
    }
}
