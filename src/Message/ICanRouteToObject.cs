//-----------------------------------------------------------------------
// <copyright file="ICanRouteToObject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    /// <summary>Implementation can be reached by object id.</summary>
    public interface ICanRouteToObject
    {
        /// <summary>Gets the object identifier.</summary>
        /// <value>Object identifier.</value>
        ulong ToObjectId { get; }
    }
}
