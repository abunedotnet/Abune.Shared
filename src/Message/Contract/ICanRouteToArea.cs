//-----------------------------------------------------------------------
// <copyright file="ICanRouteToArea.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716
namespace Abune.Shared.Message.Contract
{
    /// <summary>Implementation can be reached by area id.</summary>
    public interface ICanRouteToArea
    {
        /// <summary>Gets the area identifier.</summary>
        /// <value>Area identifier.</value>
        ulong ToAreaId { get; }
    }
}
