//-----------------------------------------------------------------------
// <copyright file="ClientAuthenticationResponse.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System.IO;
    using System.Text;

    /// <summary>Server authentication request.</summary>
    public class ClientAuthenticationResponse
    {
        /// <summary>Initializes a new instance of the <see cref="ClientAuthenticationResponse" /> class.</summary>
        public ClientAuthenticationResponse()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ClientAuthenticationResponse" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public ClientAuthenticationResponse(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    byte tokenLength = br.ReadByte();
                    byte[] tokenPayload = br.ReadBytes(tokenLength);
                    this.AuthenticationToken = Encoding.UTF8.GetString(tokenPayload);
                }
            }
        }

        /// <summary>Gets or sets the authentication token.</summary>
        /// <value>The authentication token.</value>
        public string AuthenticationToken { get; set; }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized instance.</returns>
        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(long)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    byte[] tokenPayload = Encoding.UTF8.GetBytes(this.AuthenticationToken);
                    bw.Write((byte)tokenPayload.Length);
                    bw.Write(tokenPayload);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
