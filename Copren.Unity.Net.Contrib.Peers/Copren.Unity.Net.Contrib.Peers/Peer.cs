using System;
using System.Net;
using System.Runtime.CompilerServices;
using Copren.Unity.Net.Contrib.Peers.Messages;
using Copren.Unity.Net.Domain.Messaging.Messages;

namespace Copren.Unity.Net.Contrib.Peers
{
    public class Peer
    {
        public Guid PeerId { get; }
        public Uri Uri { get; }
        public bool HostVerified { get; internal set; }
        private bool _isConnected;

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            internal set
            {
                _isConnected = value;
                ConnectedAt = LastReceivedAt = DateTimeOffset.UtcNow;
            }
        }
        public DateTimeOffset ConnectedAt { get; private set; }
        public DateTimeOffset LastReceivedAt { get; private set; }

        internal Peer(PeerIdentifyMessage peerIdentify)
        {
            PeerId = peerIdentify.PeerId;
            HostVerified = true;
            Uri = peerIdentify.Uri;
        }

        internal Peer(PeerHeloMessage peerHelo, Uri uri)
        {
            PeerId = peerHelo.PeerId;
            Uri = uri;
        }
    }
}