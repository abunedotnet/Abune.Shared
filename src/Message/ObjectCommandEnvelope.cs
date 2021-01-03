//-----------------------------------------------------------------------
// <copyright file="ObjectCommandEnvelope.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Message
{
    using System;
    using System.IO;
    using Abune.Shared.Command;
    using Abune.Shared.Message.Contract;
    using Abune.Shared.Protocol;

    /// <summary>Transport envelope for object commands.</summary>
    public class ObjectCommandEnvelope : ICanRouteToObject, ICanQuorumVote
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectCommandEnvelope" /> class.</summary>
        /// <param name="senderId">The sender identifier.</param>
        /// <param name="command">The command.</param>
        /// <param name="toObjectId">To object identifier.</param>
        public ObjectCommandEnvelope(uint senderId, BaseCommand command, ulong toObjectId)
        {
            this.SenderId = senderId;
            this.ToObjectId = toObjectId;
            this.Command = command;
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectCommandEnvelope" /> class.</summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Message is null.</exception>
        public ObjectCommandEnvelope(UdpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            using (MemoryStream stream = new MemoryStream(message.CommandBody))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    this.SenderId = br.ReadUInt32();
                    this.ToObjectId = br.ReadUInt64();
                    this.Command = new BaseCommand(br);
                }
            }
        }

        /// <summary>
        ///  Gets the sender identifier.
        /// </summary>
        /// <value>The sender identifier.</value>
        public uint SenderId { get; private set; }

        /// <summary>Gets the target object identifier.</summary>
        /// <value>Object identifier.</value>
        public ulong ToObjectId { get; private set; }

        /// <summary>Gets the command.</summary>
        /// <value>The command.</value>
        public BaseCommand Command { get; private set; }

        /// <summary>
        /// Gets the voter identifier.
        /// </summary>
        /// <value>
        /// The voter identifier.
        /// </value>
        public uint QuorumVoterId => this.SenderId;

        /// <summary>
        /// Gets the quorum hash.
        /// </summary>
        /// <value>
        /// The quorum hash.
        /// </value>
        public ulong QuorumHash => this.Command.QuorumHash;

        /// <summary>Serializes this instance.</summary>
        /// <returns>Byte serialized instance.</returns>
        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream(this.Command.Body.Length + sizeof(ushort) + (sizeof(ulong) * 2)))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(this.SenderId);
                    bw.Write(this.ToObjectId);
                    this.Command.Serialize(bw);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}
