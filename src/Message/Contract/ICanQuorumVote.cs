//-----------------------------------------------------------------------
// <copyright file="ICanQuorumVote.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716
namespace Abune.Shared.Message.Contract
{
    /// <summary>Implementation can be reached by object id.</summary>
    public interface ICanQuorumVote
    {
        /// <summary>
        /// Gets the quorum hash.
        /// </summary>
        /// <value>
        /// The quorum hash.
        /// </value>
        ulong QuorumHash { get; }

        /// <summary>
        /// Gets the voter identifier.
        /// </summary>
        /// <value>
        /// The voter identifier.
        /// </value>
        uint QuorumVoterId { get; }
    }
}
