//-----------------------------------------------------------------------
// <copyright file="ReliableUdpMessaging.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Abune.Shared.Util
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using Abune.Shared.Command;
    using Abune.Shared.Message;
    using Abune.Shared.Message.Contract;
    using Abune.Shared.Protocol;

    /// <summary>Reliable udp messaging supporting QoS 0, 1 and 2.</summary>
    public class ReliableUdpMessaging
    {
        private const int MAXSENDRETRY = 5;
        private const int MAXUNACKEDMESSAGES = 20;
        private readonly TimeSpan sendRetryInterval = TimeSpan.FromMilliseconds(50);

        private uint sequenceId;
        private System.Collections.Concurrent.ConcurrentQueue<UdpMessage> pendingMessages = new System.Collections.Concurrent.ConcurrentQueue<UdpMessage>();
        private ConcurrentDictionary<uint, MessageEntry> unAckedMessages = new ConcurrentDictionary<uint, MessageEntry>();
        private System.Collections.Concurrent.ConcurrentQueue<UdpMessage> pendingAckMessages = new System.Collections.Concurrent.ConcurrentQueue<UdpMessage>();
        private DateTime lastSentTimeStamp = DateTime.MinValue;

        /// <summary>Initializes a new instance of the <see cref="ReliableUdpMessaging" /> class.</summary>
        public ReliableUdpMessaging()
        {
            this.MTU = 500;
            this.RateLimit = 50;
        }

        /// <summary>Gets the message received count.</summary>
        /// <value>The message received count.</value>
        public int MessageReceivedCount { get; private set; }

        /// <summary>Gets the message sent count.</summary>
        /// <value>The message sent count.</value>
        public int MessageSentCount { get; private set; }

        /// <summary>Gets the deadletter count.</summary>
        /// <value>The deadletter count.</value>
        public int DeadletterCount { get; private set; }

        /// <summary>Gets the message resent count.</summary>
        /// <value>The message resent count.</value>
        public int MessageResentCount { get; private set; }

        /// <summary>Gets the message unacked count.</summary>
        /// <value>The message unacked count.</value>
        public int MessageUnackedCount
        {
            get { return this.unAckedMessages.Count; }
        }

        /// <summary>Gets or sets the mtu.</summary>
        /// <value>The mtu.</value>
        public ushort MTU { get; set; }

        /// <summary>Gets or sets the rate limit.</summary>
        /// <value>The rate limit.</value>
        public double RateLimit { get; set; }

        /// <summary>Gets or sets the callback method to process an incoming message.</summary>
        /// <value>The callback handler.</value>
        public Action<ObjectCommandEnvelope> OnProcessCommandMessage { get; set; }

        /// <summary>Gets or sets the callback method to send a udp frame.</summary>
        /// <value>The callback handler.</value>
        public Action<UdpTransferFrame> OnSendFrame { get; set; }

        /// <summary>Gets or sets the callback method for dead lettered messages.</summary>
        /// <value>The callback handler.</value>
        public Action<UdpMessage> OnDeadLetter { get; set; }

        /// <summary>Processes the message frame.</summary>
        /// <param name="frame">The frame.</param>
        /// <exception cref="ArgumentNullException">Frame is null.</exception>
        public void ProcessMessageFrame(UdpTransferFrame frame)
        {
            if (frame == null)
            {
                throw new ArgumentNullException(nameof(frame));
            }

            using (MemoryStream memstrm = new MemoryStream(frame.MessageBuffer))
            {
                using (BinaryReader br = new BinaryReader(memstrm))
                {
                    while (memstrm.Position < memstrm.Length)
                    {
                        UdpMessage udpMessage = new UdpMessage(br);
                        this.ProcessUdpMessage(udpMessage);
                    }
                }
            }
        }

        /// <summary>Sends a command.</summary>
        /// <param name="senderId">The sender identifier.</param>
        /// <param name="command">The command.</param>
        /// <param name="toObjectId">To object identifier.</param>
        /// <param name="qualityOfService">The quality of service.</param>
        public void SendCommand(uint senderId, BaseCommand command, ulong toObjectId, MessageControlFlags qualityOfService = MessageControlFlags.QOS0)
        {
            var commandEnvelope = new ObjectCommandEnvelope(senderId, command, toObjectId);
            this.SendCommandEnvelope(commandEnvelope, qualityOfService);
        }

        /// <summary>Sends a command envelope.</summary>
        /// <param name="commandEnvelope">The command envelope.</param>
        /// <param name="qualityOfService">The quality of service.</param>
        /// <exception cref="ArgumentNullException">commandEnvelope is null.</exception>
        public void SendCommandEnvelope(ObjectCommandEnvelope commandEnvelope, MessageControlFlags qualityOfService = MessageControlFlags.QOS0)
        {
            if (commandEnvelope == null)
            {
                throw new ArgumentNullException(nameof(commandEnvelope));
            }

            var message = new UdpMessage(this.sequenceId, MessageControlFlags.COMMAND | qualityOfService, commandEnvelope.Serialize());
            this.sequenceId++;
            this.pendingMessages.Enqueue(message);
        }

        /// <summary>Gets the message wait time.</summary>
        /// <returns>The time to wait.</returns>
        public TimeSpan GetMessageWaitTime()
        {
            return TimeSpan.FromMilliseconds(1000.0f / this.RateLimit);
        }

        /// <summary>Determines whether [is send rate exceeded].</summary>
        /// <returns>
        ///   <c>true</c> if [is send rate exceeded]; otherwise, <c>false</c>.</returns>
        public bool IsSendRateExceeded()
        {
            return DateTime.Now - this.lastSentTimeStamp < this.GetMessageWaitTime() || this.unAckedMessages.Count > MAXUNACKEDMESSAGES;
        }

        /// <summary>Synchronizes all pending messages between local and remote.</summary>
        /// <returns>
        /// Suggested wait time. For example when send rate is exceeded.
        /// </returns>
        public TimeSpan SynchronizeMessages()
        {
            while (this.IsMessagePending())
            {
                if (this.IsSendRateExceeded())
                {
                    return this.GetMessageWaitTime();
                }

                using (MemoryStream memstrm = new MemoryStream())
                {
                    this.AppendAckMessages(memstrm);
                    this.AppendUnAckedMessages(memstrm);
                    this.AppendPendingMessages(memstrm);

                    if (memstrm.Length > 0)
                    {
                        UdpTransferFrame frame = new UdpTransferFrame(FrameType.Message, memstrm.ToArray());
                        this.OnSendFrame(frame);
                        this.lastSentTimeStamp = DateTime.Now;
                    }
                }
            }

            return TimeSpan.Zero;
        }

        private void ProcessUdpMessage(UdpMessage message)
        {
            MessageEntry removedEntry;

            // QOS1 - at least once
            if ((message.ControlFlags & MessageControlFlags.ACK) == MessageControlFlags.ACK && this.unAckedMessages.ContainsKey(message.SequenceId))
            {
                this.unAckedMessages.TryRemove(message.SequenceId, out removedEntry);

                // QOS2 - at most once
                if ((message.ControlFlags & MessageControlFlags.QOS2) == MessageControlFlags.QOS2)
                {
                    this.pendingAckMessages.Enqueue(message);
                }
            }

            // QOS2 - at most once
            if (message.ControlFlags == MessageControlFlags.ACK2 && this.unAckedMessages.ContainsKey(message.SequenceId))
            {
                this.unAckedMessages.TryRemove(message.SequenceId, out removedEntry);
            }

            if ((message.ControlFlags & MessageControlFlags.COMMAND) == MessageControlFlags.COMMAND)
            {
                var cmdEnvelope = new ObjectCommandEnvelope(message);
                this.OnProcessCommandMessage(cmdEnvelope);

                // QOS 1 - at least once
                if ((message.ControlFlags & MessageControlFlags.QOS1) == MessageControlFlags.QOS1)
                {
                    this.pendingAckMessages.Enqueue(message);
                }
            }

            this.MessageReceivedCount++;
        }

        private bool IsMessagePending()
        {
            return !this.pendingAckMessages.IsEmpty || !this.unAckedMessages.IsEmpty || !this.pendingMessages.IsEmpty;
        }

        private void AppendAckMessages(MemoryStream memstrm)
        {
            while (!this.pendingAckMessages.IsEmpty)
            {
                UdpMessage udpMessageToAck;
                if (!this.pendingAckMessages.TryPeek(out udpMessageToAck))
                {
                    continue;
                }

                var udpAckMessage = new UdpMessage(udpMessageToAck.SequenceId, MessageControlFlags.ACK, Array.Empty<byte>());
                if (!this.AppendMessage(memstrm, udpAckMessage))
                {
                    break;
                }

                this.MessageSentCount++;
                this.pendingAckMessages.TryDequeue(out udpMessageToAck);
            }
        }

        private void AppendUnAckedMessages(MemoryStream memstrm)
        {
            var removedKeys = new List<uint>();
            foreach (var unAckedMessageEntry in this.unAckedMessages)
            {
                if (unAckedMessageEntry.Value.SendCount > MAXSENDRETRY)
                {
                    this.OnDeadLetter?.Invoke(unAckedMessageEntry.Value.Message);
                    this.DeadletterCount++;
                    removedKeys.Add(unAckedMessageEntry.Key);
                    continue;
                }

                if (unAckedMessageEntry.Value.LastSendTimeStamp + this.sendRetryInterval < DateTime.Now)
                {
                    continue;
                }

                if (!this.AppendMessage(memstrm, unAckedMessageEntry.Value.Message))
                {
                    break;
                }

                this.MessageSentCount++;
                if (unAckedMessageEntry.Value.SendCount++ > 0)
                {
                    this.MessageResentCount++;
                }

                unAckedMessageEntry.Value.LastSendTimeStamp = DateTime.Now;
            }

            MessageEntry removedEntry;
            removedKeys.ForEach(_ => this.unAckedMessages.TryRemove(_, out removedEntry));
        }

        private void AppendPendingMessages(MemoryStream memstrm)
        {
            while (!this.pendingMessages.IsEmpty)
            {
                UdpMessage udpMessage;
                if (!this.pendingMessages.TryPeek(out udpMessage))
                {
                    continue;
                }

                if (!this.AppendMessage(memstrm, udpMessage))
                {
                    break;
                }

                this.MessageSentCount++;
                if ((udpMessage.ControlFlags & MessageControlFlags.ACK) == MessageControlFlags.ACK ||
                    (udpMessage.ControlFlags & MessageControlFlags.ACK2) == MessageControlFlags.ACK2)
                {
                    this.unAckedMessages.TryAdd(udpMessage.SequenceId, new MessageEntry() { LastSendTimeStamp = DateTime.Now, Message = udpMessage, SendCount = 1 });
                }

                this.pendingMessages.TryDequeue(out udpMessage);
            }
        }

        private bool AppendMessage(MemoryStream memstrm, UdpMessage udpMessage)
        {
            byte[] udpMessagePayload = udpMessage.Serialize();
            if (memstrm.Length + udpMessagePayload.Length > this.MTU)
            {
                return false;
            }

            memstrm.Write(udpMessagePayload, 0, udpMessagePayload.Length);
            return true;
        }

        private class MessageEntry
        {
            /// <summary>Gets or sets the last send time stamp.</summary>
            /// <value>The last send time stamp.</value>
            public DateTime LastSendTimeStamp { get; set; }

            /// <summary>Gets or sets the send count.</summary>
            /// <value>The send count.</value>
            public ushort SendCount { get; set; }

            /// <summary>Gets or sets the message.</summary>
            /// <value>The message.</value>
            public UdpMessage Message { get; set; }
        }
    }
}
