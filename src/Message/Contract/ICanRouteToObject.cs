//-----------------------------------------------------------------------
// <copyright file="ICanRouteToObject.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716
namespace Abune.Shared.Message.Contract
{
    /// <summary>Implementation can be reached by object id.</summary>
    public interface ICanRouteToObject
    {
        /// <summary>Gets the object identifier.</summary>
        /// <value>Object identifier.</value>
        ulong ToObjectId { get; }
    }
}
