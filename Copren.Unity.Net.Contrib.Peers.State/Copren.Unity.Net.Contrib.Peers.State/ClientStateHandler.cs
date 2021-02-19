using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Copren.Unity.Net.Contrib.Peers.State.Messages;
using Copren.Unity.Net.Contrib.State;
using Copren.Unity.Net.Core.Connection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Copren.Unity.Net.Contrib.Peers.State
{
    public class ClientStateHandler : IClientStateHandler
    {
        private readonly ILogger _logger;
        private readonly PeerCollection _peerCollection;
        private readonly IServiceProvider _services;
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

        public ClientStateHandler(ILogger logger, PeerCollection peerCollection, IServiceProvider services)
        {
            _logger = logger.ForContext<ClientStateHandler>();
            _peerCollection = peerCollection;
            _services = services;
        }

        public Task OnStateChanged(Client client, bool fromServer, object state)
        {
            // We don't broadcast server updates - let the server do that
            if (fromServer) return Task.CompletedTask;

            _logger.Verbose("Broadcasting {ClientId:s} state change to peers", client.Id);
            return Client.BroadcastPeerMessageAsync(
                new UpdatePeerStateMessage(false, client.Id, state));
        }
    }
}