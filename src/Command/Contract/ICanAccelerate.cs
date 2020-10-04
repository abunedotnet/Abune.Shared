//-----------------------------------------------------------------------
// <copyright file="ICanAccelerate.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716
namespace Abune.Shared.Command.Contract
{
    /// <summary>
    /// Interface for things that could accelerate.
    /// </summary>
    public interface ICanAccelerate
    {
        /// <summary>
        /// Gets the velocity on x axis in units per second.
        /// </summary>
        float VelocityX { get; }

        /// <summary>
        /// Gets the velocity on y axis in units per second.
        /// </summary>
        float VelocityY { get; }

        /// <summary>
        /// Gets the velocity on z axis in units per second.
        /// </summary>
        float VelocityZ { get; }

        /// <summary>
        /// Gets the angular velocity on x axis in radiants per second.
        /// </summary>
        float AngularVelocityX { get; }

        /// <summary>
        /// Gets the angular velocity on x axis in radiants per second.
        /// </summary>
        float AngularVelocityY { get; }

        /// <summary>
        /// Gets the angular velocity on x axis in radiants per second.
        /// </summary>
        float AngularVelocityZ { get; }
    }
}
