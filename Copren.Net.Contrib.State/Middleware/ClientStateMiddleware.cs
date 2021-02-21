using System;
using System.Threading.Tasks;
using Copren.Net.Contrib.State.Messages;
using Copren.Net.Core.Context;
using Copren.Net.Core.Middleware;
using Copren.Net.Domain.Messaging.Messages;

namespace Copren.Net.Contrib.State
{
    public class ClientStateMiddleware : IMiddleware
    {
        private readonly ClientStateManager _stateManager;

        public ClientStateMiddleware(ClientStateManager stateManager)
        {
            _stateManager = stateManager;
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
            if (message is UpdateClientStateMessage updateMessage)
            {
                return _stateManager.FireUpdateEvent(true, updateMessage.State);
            }

            return next();
        }
    }
}