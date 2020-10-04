//-----------------------------------------------------------------------
// <copyright file="ICanRouteToArea.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    /// <summary>Implementation can be reached by area id.</summary>
    public interface ICanRouteToArea
    {
        /// <summary>Gets the area identifier.</summary>
        /// <value>Area identifier.</value>
        ulong ToAreaId { get; }
    }
}
