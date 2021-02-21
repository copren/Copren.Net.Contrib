using System;
using System.Threading.Tasks;
using Copren.Net.Core.Connection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Copren.Net.Contrib.State
{
    public class ClientStateBuilder
    {
        public ClientBuilder ClientBuilder { get; }

        public ClientStateBuilder(ClientBuilder builder)
        {
            ClientBuilder = builder;
        }

        public ClientStateBuilder AddStateListener<T>(T handler)
            where T : class, IClientStateHandler
        {
            ClientBuilder.Configure(s =>
            {
                s.TryAddEnumerable(
                    ServiceDescriptor.Singleton<IClientStateHandler, T>(p => handler));
            });
            return this;
        }

        public ClientStateBuilder AddStateListener<T>()
            where T : IClientStateHandler
        {
            ClientBuilder.Configure(s =>
            {
                s.TryAddEnumerable(
                    ServiceDescriptor.Describe(
                        typeof(IClientStateHandler),
                        typeof(T),
                        ServiceLifetime.Singleton));
            });
            return this;
        }
    }
}