//-----------------------------------------------------------------------
// <copyright file="IObjectCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716
namespace Abune.Shared.Command.Contract
{
    /// <summary>
    /// Interface for object commands.
    /// </summary>
    public interface IObjectCommand
    {
        /// <summary>
        /// Gets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        ulong ObjectId { get; }
    }
}