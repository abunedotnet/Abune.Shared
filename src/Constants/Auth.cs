//-----------------------------------------------------------------------
// <copyright file="Auth.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716

namespace Abune.Shared.Constants
{
    /// <summary>
    /// Constants for authentication.
    /// </summary>
    public static class Auth
    {
        /// <summary>
        /// Constants for jwt claims.
        /// </summary>
#pragma warning disable CA1034 // Geschachtelte Typen dürfen nicht sichtbar sein
        public static class JwtClaims
#pragma warning restore CA1034 // Geschachtelte Typen dürfen nicht sichtbar sein
        {
            /// <summary>
            /// The authentication challenge.
            /// </summary>
            public const string AUTHENTICATIONCHALLENGE = "abnch";
        }
    }
}
