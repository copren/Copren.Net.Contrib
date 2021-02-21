using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Copren.Net.Core.Connection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Copren.Net.Contrib.Peers.State
{
    public class PeerStateManager
    {
        private Client _client;
        private Client Client
        {
            get
            {
                if (_client == null)
                {
                    _client = _services.GetRequiredService<Client>();
                }

                return _client;
            }
        }
        private readonly ILogger _logger;
        private readonly IDictionary<Guid, object> _peerState = new Dictionary<Guid, object>();
        private readonly PeerCollection _peerCollection;
        private readonly IEnumerable<IPeerStateHandler> _peerStateHandlers;
        private readonly IServiceProvider _services;

        public PeerStateManager(
            ILogger logger,
            PeerCollection peerCollection,
            IEnumerable<IPeerStateHandler> peerStateHandlers,
            IServiceProvider services)
        {
            _logger = logger.ForContext<PeerStateManager>();
            _peerCollection = peerCollection;
            _peerStateHandlers = peerStateHandlers;
            _services = services;
        }

        internal Task UpdatePeerState(Guid peerId, object state)
        {
            _logger.Verbose("Updating {PeerId:s} state", peerId);
            _peerState[peerId] = state;
            var tasks = new List<Task>();
            foreach (var handler in _peerStateHandlers)
            {
                tasks.Add(handler.OnPeerStateChanged(Client, peerId, state));
            }
            return Task.WhenAll(tasks);
        }

        public bool TryGet(Guid peerId, out object state)
        {
            return _peerState.TryGetValue(peerId, out state);
        }
    }
}