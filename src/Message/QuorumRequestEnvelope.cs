//-----------------------------------------------------------------------
// <copyright file="QuorumRequestEnvelope.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using Abune.Shared.Message.Contract;

    /// <summary>Transport envelope for object commands.</summary>
    public class QuorumRequestEnvelope : ICanRouteToArea, ICanRouteToObject
    {
        /// <summary>
        /// The unknown voter count constant.
        /// </summary>
        public const int UNKNOWNVOTERCOUNT = -1;

        /// <summary>Initializes a new instance of the <see cref="QuorumRequestEnvelope" /> class.</summary>
        /// <param name="commandEnvelope">The command envelope.</param>
        public QuorumRequestEnvelope(ObjectCommandEnvelope commandEnvelope)
        {
            this.CommandEnvelope = commandEnvelope;
            this.VoterCount = UNKNOWNVOTERCOUNT;
        }

        /// <summary>Gets the area identifier.</summary>
        /// <value>Area identifier.</value>
        public ulong ToAreaId => this.VotingAreaId;

        /// <summary>Gets the target object identifier.</summary>
        /// <value>Object identifier.</value>
        public ulong ToObjectId => this.CommandEnvelope.ToObjectId;

        /// <summary>Gets or sets the area identifier where the vote happens.</summary>
        /// <value>Area identifier of voting.</value>
        public ulong VotingAreaId { get; set; }

        /// <summary>
        /// Gets or sets the voter count.
        /// </summary>
        /// <value>
        /// The voter count.
        /// </value>
        public int VoterCount { get; set; }

        /// <summary>
        ///  Gets the object command envelope.
        /// </summary>
        /// <value>The object command envelope.</value>
        public ObjectCommandEnvelope CommandEnvelope { get; private set; }
    }
}
