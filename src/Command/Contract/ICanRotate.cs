//-----------------------------------------------------------------------
// <copyright file="ICanRotate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command.Contract
{
    /// <summary>Implementation can rotate (Quaternion).</summary>
    public interface ICanRotate
    {
        /// <summary>Gets the rotation w.</summary>
        /// <value>The rotation w.</value>
        float RotationW { get; }

        /// <summary>Gets the rotation x.</summary>
        /// <value>The rotation x.</value>
        float RotationX { get; }

        /// <summary>Gets the rotation y.</summary>
        /// <value>The rotation y.</value>
        float RotationY { get; }

        /// <summary>Gets the rotation z.</summary>
        /// <value>The rotation z.</value>
        float RotationZ { get; }
    }
}
