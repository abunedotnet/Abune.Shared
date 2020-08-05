//-----------------------------------------------------------------------
// <copyright file="AreaCommandEnvelope.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CA1716

namespace Abune.Shared.Message
{
    using System.IO;

    /// <summary>Envelope for area commands.</summary>
    public class AreaCommandEnvelope : ICanRouteToArea, ICanRouteToObject
    {
        /// <summary>Initializes a new instance of the <see cref="AreaCommandEnvelope" /> class.</summary>
        /// <param name="toAreaId">To area identifier.</param>
        /// <param name="objectCommandEnvelope">The object command envelope.</param>
        public AreaCommandEnvelope(ulong toAreaId, ObjectCommandEnvelope objectCommandEnvelope)
        {
            this.ToAreaId = toAreaId;
            this.ObjectCommandEnvelope = objectCommandEnvelope;
        }

        /// <summary>Initializes a new instance of the <see cref="AreaCommandEnvelope" /> class.</summary>
        /// <param name="data">The data.</param>
        public AreaCommandEnvelope(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    /*ushort reserve = br.ReadUInt16();
                    FromObjectId = br.ReadUInt64();
                    ToObjectId = br.ReadUInt64();
                    uint commandType = br.ReadUInt32();
                    uint commandLength = br.ReadUInt32();
                    byte[] commandData = br.ReadBytes((int)commandLength);
                    Command = new BaseCommand((CommandType)commandType, commandData);                    */
                }
            }
        }

        /// <summary>Gets the area identifier to route to.</summary>
        /// <value>Area identifier.</value>
        public ulong ToAreaId { get; private set; }

        /// <summary>Gets the object command envelope.</summary>
        /// <value>The object command envelope.</value>
        public ObjectCommandEnvelope ObjectCommandEnvelope { get; private set; }

        /// <summary>  Gets the object identifier to route to.</summary>
        /// <value>Object identifier.</value>
        public ulong ToObjectId
        {
            get
            {
                return this.ObjectCommandEnvelope.ToObjectId;
            }
        }
    }
}
