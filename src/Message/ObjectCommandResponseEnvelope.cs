//-----------------------------------------------------------------------
// <copyright file="ObjectCommandResponseEnvelope.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    /// <summary>Transport envolope for response envelopes.</summary>
    /// <seealso cref="ObjectCommandEnvelope"/>
    public class ObjectCommandResponseEnvelope
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectCommandResponseEnvelope" /> class.</summary>
        /// <param name="message">The message.</param>
        public ObjectCommandResponseEnvelope(ObjectCommandEnvelope message)
        {
            this.Message = message;
        }

        /// <summary>Gets the message.</summary>
        /// <value>The message.</value>
        public ObjectCommandEnvelope Message { get; private set; }
    }
}
