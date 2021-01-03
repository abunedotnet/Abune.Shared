//-----------------------------------------------------------------------
// <copyright file="CommandFlags.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    using System;

    #pragma warning disable CA1028
    /// <summary>
    /// Different command types.
    /// </summary>
    [Flags]
    public enum CommandFlags : byte
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Requests a successful quorum to before updating.
        /// </summary>
        QuorumRequest = 0x01,
    }
}
