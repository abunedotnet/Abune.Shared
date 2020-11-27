//-----------------------------------------------------------------------
// <copyright file="ICanAccelerate.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716
namespace Abune.Shared.Command.Contract
{
    using Abune.Shared.DataType;

    /// <summary>
    /// Interface for things that could accelerate.
    /// </summary>
    public interface ICanAccelerate
    {
        /// <summary>
        /// Gets the velocity in units per second.
        /// </summary>
        AVector3 Velocity { get; }

        /// <summary>
        /// Gets the angular velocity in radiants per second.
        /// </summary>
        AVector3 AngularVelocity { get; }
    }
}
