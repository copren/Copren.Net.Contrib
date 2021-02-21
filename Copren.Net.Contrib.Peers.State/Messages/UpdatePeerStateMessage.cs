using System;
using Copren.Net.Domain.Messaging.Messages;
using ProtoBuf;

namespace Copren.Net.Contrib.Peers.State.Messages
{
    [ProtoContract]
    public class UpdatePeerStateMessage : Message
    {
        [ProtoMember(1)]
        public bool FromServer { get; }
        [ProtoMember(2)]
        public Guid PeerId { get; }
        [ProtoMember(3, DynamicType = true)]
        public object State { get; }

        public UpdatePeerStateMessage() { }

        public UpdatePeerStateMessage(bool fromServer, Guid peerId, object state)
        {
            FromServer = fromServer;
            PeerId = peerId;
            State = state;
        }
    }
}