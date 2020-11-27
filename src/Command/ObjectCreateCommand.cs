//-----------------------------------------------------------------------
// <copyright file="ObjectCreateCommand.cs" company="Thomas Stollenwerk (motmot80)">
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
                    this.TargetPosition = ReadVector3(br);
                    this.TargetOrientation = ReadQuaternion(br);
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
        /// <param name="targetPosition">Position where object is located.</param>
        /// <param name="targetOrientation">Quaternion orientation of object.</param>
        public ObjectCreateCommand(
            ulong frameTick,
            ulong objectId,
            ulong parentObjectId,
            uint ownerId,
            uint typeId,
            AVector3 targetPosition,
            AQuaternion targetOrientation)
            : base(CommandType.ObjectCreate)
        {
            if (targetPosition == null)
            {
                throw new ArgumentNullException(nameof(targetPosition));
            }

            if (targetOrientation == null)
            {
                throw new ArgumentNullException(nameof(targetOrientation));
            }

            this.FrameTick = frameTick;
            this.ObjectId = objectId;
            this.ParentObjectId = parentObjectId;
            this.OwnerId = ownerId;
            this.TypeId = typeId;
            this.TargetPosition = targetPosition;
            this.TargetOrientation = targetOrientation;
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
                    Write(bw, this.TargetPosition);
                    Write(bw, this.TargetOrientation);
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
        /// Gets or sets position where object is located.
        /// </summary>
        public AVector3 TargetPosition { get; set; }

        /// <summary>
        /// Gets or sets quaternion orientation of object.
        /// </summary>
        public AQuaternion TargetOrientation { get; set; }

#pragma warning disable CA1033 // convert meaning

        /// <summary>
        /// Gets position where object is located.
        /// </summary>
        AVector3 ICanLocate.WorldPosition
        {
            get
            {
                return this.TargetPosition;
            }
        }

        /// <summary>
        /// Gets quaternion direction w axis of object.
        /// </summary>
        AQuaternion ICanRotate.Orientation
        {
            get
            {
                return this.TargetOrientation;
            }
        }
    }
}
