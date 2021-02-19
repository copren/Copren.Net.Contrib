using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Copren.Unity.Net.Contrib.Peers.State.Messages;
using Copren.Unity.Net.Contrib.State;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Copren.Unity.Net.Hosting;
using Copren.Unity.Net.Hosting.Hosting;

namespace Copren.Unity.Net.Contrib.Peers.State
{
    public class HostStateHandler : IHostStateHandler
    {
        private readonly ClientCollection _clientCollection;
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private Host _host;
        private Host Host
        {
            get
            {
                if (_host == null)
                {
                    _host = _services.GetRequiredService<Host>();
                }

                return _host;
            }
        }

        public HostStateHandler(
            ClientCollection clientCollection,
            ILogger logger,
            IServiceProvider services)
        {
            _clientCollection = clientCollection;
            _logger = logger.ForContext<HostStateHandler>();
            _services = services;
        }

        public Task OnStateChanged(Host host, bool fromServer, Guid clientId, object state)
        {
            // Skip any messages from the client - we'll let them broadcast those
            if (!fromServer) return Task.CompletedTask;

            _logger.Verbose("Broadcasting {ClientId:s} state change to clients", clientId);
            return Host.BroadcastClientMessageAsync(
                new UpdatePeerStateMessage(true, clientId, state),
                new HashSet<Guid>(new Guid[] { clientId }));
        }
    }
}