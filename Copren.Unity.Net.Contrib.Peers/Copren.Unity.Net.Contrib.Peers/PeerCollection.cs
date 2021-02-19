using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using Copren.Unity.Net.Domain;

namespace Copren.Unity.Net.Contrib.Peers
{
    public class PeerCollection : IEnumerable<Peer>
    {
        private IDictionary<Uri, Peer> _endPointToPeerMapping = new Dictionary<Uri, Peer>();
        private IDictionary<Guid, Peer> _guidToPeerMapping = new Dictionary<Guid, Peer>();

        public void Add(Peer peer)
        {
            _endPointToPeerMapping[peer.Uri] = peer;
            _guidToPeerMapping[peer.PeerId] = peer;
        }

        public void Remove(Peer peer)
        {
            _endPointToPeerMapping.Remove(peer.Uri);
            _guidToPeerMapping.Remove(peer.PeerId);
        }

        public Peer Get(Guid peerId)
        {
            return _guidToPeerMapping[peerId];
        }

        public bool TryGet(Guid peerId, out Peer peer)
        {
            peer = null;
            if (!Contains(peerId)) return false;
            peer = Get(peerId);
            return true;
        }

        public Peer Get(Uri uri)
        {
            return _endPointToPeerMapping[uri];
        }

        public bool TryGet(Uri uri, out Peer peer)
        {
            peer = null;
            if (!Contains(uri)) return false;
            peer = Get(uri);
            return true;
        }

        public bool Contains(Guid peerId)
        {
            return _guidToPeerMapping.ContainsKey(peerId);
        }

        public bool Contains(Uri uri)
        {
            return _endPointToPeerMapping.ContainsKey(uri);
        }

        public IEnumerator<Peer> GetEnumerator()
        {
            return _guidToPeerMapping.Values.ToImmutableList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _guidToPeerMapping.Values.ToImmutableList().GetEnumerator();
        }
    }
}