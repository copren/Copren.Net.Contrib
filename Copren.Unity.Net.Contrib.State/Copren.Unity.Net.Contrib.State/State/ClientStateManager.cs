using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Copren.Unity.Net.Contrib.State.Messages;
using Copren.Unity.Net.Core.Connection;
using Copren.Unity.Net.Core.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Copren.Unity.Net.Contrib.State
{
    public class ClientStateManager
    {
        private object _state;
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private readonly IEnumerable<IClientStateHandler> _stateHandlers;
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

        public ClientStateManager(
            ILogger logger,
            IServiceProvider services,
            IEnumerable<IClientStateHandler> stateHandlers)
        {
            _logger = logger.ForContext<ClientStateManager>();
            _services = services;
            _stateHandlers = stateHandlers;
        }

        public async Task UpdateAsync(object state)
        {
            _logger.Verbose("{ClientId:s} is updating state", Client.Id);

            lock (this)
            {
                _state = state;
            }

            await Client.SendServerMessageAsync(
                new UpdateClientStateMessage(false, state));
            await FireUpdateEvent(false, state);
        }

        public object GetState()
        {
            return _state;
        }

        internal async Task FireUpdateEvent(bool fromServer, object state)
        {
            if (fromServer)
            {
                _logger.Information("Received updated state from server for {ClientId:s}", Client.Id);
                lock (this)
                {
                    _state = state;
                }
            }

            foreach (var handler in _stateHandlers)
            {
                await handler?.OnStateChanged(Client, fromServer, state);
            }
        }
    }
}