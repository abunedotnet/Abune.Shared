//-----------------------------------------------------------------------
// <copyright file="ICanRotate.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command.Contract
{
    using Abune.Shared.DataType;

    /// <summary>Implementation can rotate (Quaternion).</summary>
    public interface ICanRotate
    {
        /// <summary>Gets the orientation.</summary>
        /// <value>The orientation.</value>
        AQuaternion Orientation { get; }
    }
}
