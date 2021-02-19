using System;
using System.Threading.Tasks;
using Copren.Unity.Net.Contrib.Peers.State.Messages;
using Copren.Unity.Net.Core.Context;
using Copren.Unity.Net.Core.Middleware;
using Copren.Unity.Net.Domain.Messaging.Messages;
using Serilog;

namespace Copren.Unity.Net.Contrib.Peers.State
{
    public class PeerStateMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        private readonly PeerStateManager _peerStateManager;

        public PeerStateMiddleware(ILogger logger, PeerStateManager peerStateManager)
        {
            _logger = logger.ForContext<PeerStateMiddleware>();
            _peerStateManager = peerStateManager;
        }

        public Task OnConnected(ClientContext context, Func<Task> next)
        {
            return next();
        }

        public Task OnDisconnected(ClientContext context, Func<Task> next)
        {
            return next();
        }

        public Task OnMessage(ClientContext context, Message message, Func<Task> next)
        {
            if (message is UpdatePeerStateMessage updateMessage)
            {
                _logger.Verbose("Received {PeerId:s} state update", updateMessage.PeerId);
                return _peerStateManager.UpdatePeerState(updateMessage.PeerId, updateMessage.State);
            }

            return next();
        }
    }
}