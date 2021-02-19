using System;
using System.Threading.Tasks;
using Copren.Unity.Net.Contrib.State.Messages;
using Copren.Unity.Net.Domain;
using Copren.Unity.Net.Domain.Messaging.Messages;
using Copren.Unity.Net.Hosting.Context;
using Copren.Unity.Net.Hosting.Middleware;

namespace Copren.Unity.Net.Contrib.State
{
    public class HostStateMiddleware : IMiddleware
    {
        private readonly HostStateManager _stateManager;

        public HostStateMiddleware(HostStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public Task OnClientConnected(HostContext context, Client client, Func<Task> next)
        {
            return next();
        }

        public Task OnClientDisconnected(HostContext context, Client client, Func<Task> next)
        {
            return next();
        }

        public Task OnMessage(HostContext context, Message message, Func<Task> next)
        {
            if (message is UpdateClientStateMessage updateMessage && context.ClientId.HasValue)
            {
                return _stateManager.FireUpdateEvent(false, context.ClientId.Value, updateMessage.State);
            }

            return next();
        }
    }
}