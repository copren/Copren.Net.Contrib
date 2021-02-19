using System;
using System.Net;
using Copren.Unity.Net.Domain.Messaging.Messages;
using ProtoBuf;

namespace Copren.Unity.Net.Contrib.Peers.Messages
{
    [ProtoContract]
    public class PeerHeloMessage : Message
    {
        [ProtoMember(1)]
        public Guid PeerId { get; set; }
    }
}