//-----------------------------------------------------------------------
// <copyright file="IAreaCommand.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716
namespace Abune.Shared.Command.Contract
{
    /// <summary>
    /// Interface for area commands.
    /// </summary>
    public interface IAreaCommand
    {
        /// <summary>
        /// Gets the area identifier.
        /// </summary>
        /// <value>
        /// The area identifier.
        /// </value>
        ulong AreaId { get; }
    }
}
