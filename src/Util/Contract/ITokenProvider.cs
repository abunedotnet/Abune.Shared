//-----------------------------------------------------------------------
// <copyright file="ITokenProvider.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CA1716

namespace Assets.Abune.Client.Unity.Shared.Util.Contract
{
    using System;

    /// <summary>
    /// Interface for token provider.
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>Creates the JWT token.</summary>
        /// <param name="authenticationChallenge">The authentication challenge.</param>
        /// <param name="expires">The expires.</param>
        /// <returns>Json web token.</returns>
        string CreateJWTToken(string authenticationChallenge, DateTime expires);
    }
}
