using System;
using Copren.Net.Domain.Messaging.Messages;
using ProtoBuf;

namespace Copren.Net.Contrib.State.Messages
{
    [ProtoContract]
    public class UpdateClientStateMessage : Message
    {
        [ProtoMember(1)]
        public bool FromServer { get; }
        [ProtoMember(2, DynamicType = true)]
        public object State { get; }

        public UpdateClientStateMessage() { }

        public UpdateClientStateMessage(bool fromServer, object state)
        {
            FromServer = fromServer;
            State = state;
        }
    }
}