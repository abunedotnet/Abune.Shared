//-----------------------------------------------------------------------
// <copyright file="ServerAuthenticationRequest.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System.IO;
    using System.Text;

    /// <summary>Server authentication request.</summary>
    public class ServerAuthenticationRequest
    {
        /// <summary>Initializes a new instance of the <see cref="ServerAuthenticationRequest" /> class.</summary>
        public ServerAuthenticationRequest()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ServerAuthenticationRequest" /> class.</summary>
        /// <param name="buffer">The buffer.</param>
        public ServerAuthenticationRequest(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    byte messageLength = br.ReadByte();
                    byte[] messagePayload = br.ReadBytes(messageLength);
                    this.AuthenticationChallenge = Encoding.UTF8.GetString(messagePayload);
                }
            }
        }

        /// <summary>Gets or sets the welcome message.</summary>
        /// <value>The message.</value>
        public string AuthenticationChallenge { get; set; }

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized instance.</returns>
        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(sizeof(long)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    byte[] authChallengePayload = Encoding.UTF8.GetBytes(this.AuthenticationChallenge);
                    bw.Write((byte)authChallengePayload.Length);
                    bw.Write(authChallengePayload);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
