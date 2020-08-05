//-----------------------------------------------------------------------
// <copyright file="StartServerMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System.Net;

    /// <summary>Server start message.</summary>
    public class StartServerMessage
    {
        /// <summary>Initializes a new instance of the <see cref="StartServerMessage" /> class.</summary>
        /// <param name="serverEndpoint">The server endpoint.</param>
        public StartServerMessage(IPEndPoint serverEndpoint)
        {
            this.ServerEndpoint = serverEndpoint;
        }

        /// <summary>Gets the server endpoint.</summary>
        /// <value>The server endpoint.</value>
        public IPEndPoint ServerEndpoint { get; private set; }
    }
}
