//-----------------------------------------------------------------------
// <copyright file="Base64Url.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Assets.Abune.Client.Unity.Shared.Util
{
    using System;

    /// <summary>
    /// Helper class to support base 64 url encoding
    /// </summary>
    public static class Base64Url
    {
        /// <summary>
        /// Transforms byte array to a base 64 url encoded string.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>Base 64 url encoded data.</returns>
        public static string Encode(byte[] data)
        {
            var output = Convert.ToBase64String(data);
            return RemoveTrailingChars(output).Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Transforms base 64 url encoded string to byte array.
        /// </summary>
        /// <param name="base64UrlData">The base 64 url encoded data.</param>
        /// <returns>The byte array.</returns>
        public static byte[] Decode(string base64UrlData)
        {
            var reverted = base64UrlData.Replace('-', '+').Replace('_', '/');
            string b64data = AppendTrailingChars(reverted);
            return Convert.FromBase64String(b64data); 
        }

        private static string AppendTrailingChars(string data)
        {
            switch (data.Length % 4)
            {
                case 0: return data;
                case 2: return data + "=="; 
                case 3: return data + "="; 
                default: throw new ArgumentException(nameof(data));
            }
        }

        private static string RemoveTrailingChars(string data)
        {
            return data.Split('=')[0];
        }
    }
}
