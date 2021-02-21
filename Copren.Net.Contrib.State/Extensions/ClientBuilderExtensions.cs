using System;
using Copren.Net.Core.Connection;
using Microsoft.Extensions.DependencyInjection;

namespace Copren.Net.Contrib.State
{
    public static class ClientBuilderExtensions
    {
        public static ClientStateBuilder AddState(this ClientBuilder self)
        {
            self.Configure(s =>
            {
                s.AddSingleton<ClientStateManager>();
            });
            self.AddMiddleware<ClientStateMiddleware>();
            return new ClientStateBuilder(self);
        }
    }
}
