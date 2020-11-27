//-----------------------------------------------------------------------
// <copyright file="ICanLocate.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command.Contract
{
    using Abune.Shared.DataType;

    /// <summary>
    /// Interface for things that can be located.
    /// </summary>
    public interface ICanLocate
    {
        /// <summary>
        /// Gets the world position.
        /// </summary>
        AVector3 WorldPosition { get; }
    }
}
