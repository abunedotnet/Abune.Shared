//-----------------------------------------------------------------------
// <copyright file="ICanLocate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command.Contract
{
    /// <summary>
    /// Interface for things that can be located.
    /// </summary>
    public interface ICanLocate
    {
        /// <summary>
        /// Gets the world position x axis.
        /// </summary>
        float WorldPositionX { get; }

        /// <summary>
        /// Gets the world position y axis.
        /// </summary>
        float WorldPositionY { get; }

        /// <summary>
        /// Gets the world position z axis.
        /// </summary>
        float WorldPositionZ { get; }
    }
}
