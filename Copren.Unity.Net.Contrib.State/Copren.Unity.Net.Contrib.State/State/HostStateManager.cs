using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Copren.Unity.Net.Contrib.State.Messages;
using Copren.Unity.Net.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Copren.Unity.Net.Hosting.Hosting;

namespace Copren.Unity.Net.Contrib.State
{
    public class HostStateManager
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private readonly IDictionary<Guid, object> _state = new Dictionary<Guid, object>();
        private readonly IEnumerable<IHostStateHandler> _stateHandlers;

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

        public HostStateManager(
            ILogger logger,
            IServiceProvider services,
            IEnumerable<IHostStateHandler> stateHandler)
        {
            _logger = logger.ForContext<HostStateManager>();
            _services = services;
            _stateHandlers = stateHandler;
        }

        public async Task UpdateAsync(Guid clientId, object state)
        {
            try
            {
                _logger.Verbose("Host is updating {ClientId:s} state", clientId);
                _state[clientId] = state;
                await Host.SendClientMessageAsync(
                    clientId,
                    new UpdateClientStateMessage(
                        true,
                        state));
                await FireUpdateEvent(false, clientId, state);
            }
            catch (Exception e)
            {
                _logger.Error(e, "{Exception}", e.Message);
            }
        }

        public bool TryGetState(Guid clientId, out object state)
        {
            return _state.TryGetValue(clientId, out state);
        }

        internal async Task FireUpdateEvent(bool fromServer, Guid clientId, object state)
        {
            if (_stateHandlers == null) return;

            foreach (var handler in _stateHandlers)
            {
                await handler?.OnStateChanged(Host, fromServer, clientId, state);
            }
        }
    }
}