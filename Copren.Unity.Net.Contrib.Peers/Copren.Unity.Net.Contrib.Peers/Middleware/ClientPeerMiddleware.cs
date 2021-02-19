using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Copren.Unity.Net.Contrib.Peers.Messages;
using Copren.Unity.Net.Core.Context;
using Copren.Unity.Net.Core.Messaging;
using Copren.Unity.Net.Core.Middleware;
using Copren.Unity.Net.Domain.Messaging.Messages;
using Serilog;

namespace Copren.Unity.Net.Contrib.Peers
{
    public class ClientPeerMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        private readonly MessageCenter _messageCenter;
        private readonly PeerCollection _peerCollection;
        private readonly IEnumerable<IPeerHandler> _peerHandlers;

        public ClientPeerMiddleware(
            ILogger logger,
            MessageCenter messageCenter,
            PeerCollection peerCollection,
            IEnumerable<IPeerHandler> peerHandlers)
        {
            _logger = logger.ForContext<ClientPeerMiddleware>();
            _messageCenter = messageCenter;
            _peerCollection = peerCollection;
            _peerHandlers = peerHandlers;
        }

        public Task OnConnected(ClientContext context, Func<Task> next)
        {
            return next();
        }

        public Task OnDisconnected(ClientContext context, Func<Task> next)
        {
            return next();
        }

        public async Task OnMessage(ClientContext context, Message message, Func<Task> next)
        {
            if (!context.Client.IsConnected) return;

            switch (message)
            {
                case PeerIdentifyMessage newPeer:
                    {
                        Peer peer;
                        if (!_peerCollection.TryGet(newPeer.PeerId, out peer))
                        {
                            peer = new Peer(newPeer);
                            _peerCollection.Add(peer);
                        }

                        _logger.Information("{ClientId:s}: Received peer identity {PeerId} -> {Uri}", context.Client.Id, peer.PeerId, peer.Uri);

                        // Send the peer a helo
                        _logger.Verbose("{ClientId:s}: Sending peer {PeerId} HELO -> {Uri}", context.Client.Id, peer.PeerId, peer.Uri);
                        await _messageCenter.SendMessageAsync(peer.Uri, new PeerHeloMessage
                        {
                            PeerId = context.Client.Id
                        });
                        break;
                    }
                case PeerHeloMessage peerHelo:
                    {
                        Peer peer;
                        if (!_peerCollection.TryGet(peerHelo.PeerId, out peer))
                        {
                            peer = new Peer(peerHelo, context.SenderUri);
                            _peerCollection.Add(peer);
                        }

                        _logger.Information("{ClientId:s}: Received peer HELO {PeerId} -> {Uri}", context.Client.Id, peer.PeerId, peer.Uri);
                        peer.IsConnected = true;

                        foreach (var handler in _peerHandlers)
                        {
                            await handler?.OnPeerConnected(peer.PeerId);
                        }

                        break;
                    }
            }

            await next();
        }
    }
}
