//-----------------------------------------------------------------------
// <copyright file="ObjectCommandRequestEnvelope.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    /// <summary>Transport envolope for request envelopes.</summary>
    /// <seealso cref="ObjectCommandEnvelope"/>
    public class ObjectCommandRequestEnvelope
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectCommandRequestEnvelope" /> class.</summary>
        /// <param name="message">The message.</param>
        public ObjectCommandRequestEnvelope(ObjectCommandEnvelope message)
        {
            this.Message = message;
        }

        /// <summary>
        ///   <para>
        /// Gets the message.
        /// </para>
        /// </summary>
        /// <value>The message.</value>
        public ObjectCommandEnvelope Message { get; private set; }
    }
}
