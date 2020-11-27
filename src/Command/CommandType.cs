//-----------------------------------------------------------------------
// <copyright file="CommandType.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Command
{
    #pragma warning disable CA1028
    /// <summary>
    /// Different command types.
    /// </summary>
    public enum CommandType : uint
    {
        /// <summary>
        /// Updates object position.
        /// </summary>
        ObjectUpdatePosition = 0x07,

        /// <summary>
        /// Creates an object.
        /// </summary>
        ObjectCreate = 0x08,

        /// <summary>
        /// Destroys an object.
        /// </summary>
        ObjectDestroy = 0x09,

        /// <summary>
        /// Subscribes client to an area.
        /// </summary>
        SubscribeArea = 0x10,

        /// <summary>
        /// Unsubscribes client from an area.
        /// </summary>
        UnsubscribeArea = 0x11,

        /// <summary>
        /// Requests to lock an object.
        /// </summary>
        ObjectLock = 0x12,

        /// <summary>
        /// Requests to unlock an object.
        /// </summary>
        ObjectUnlock = 0x13,

        /// <summary>
        /// Updates or creates an object value.
        /// </summary>
        ObjectValueUpdate = 0x14,

        /// <summary>
        /// Deletes an object value.
        /// </summary>
        ObjectValueRemove = 0x15,
    }
}
