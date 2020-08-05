//-----------------------------------------------------------------------
// <copyright file="ICanRouteToArea.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
