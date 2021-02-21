using Copren.Net.Core.Connection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Copren.Net.Contrib.Peers
{
    public class ClientPeerBuilder
    {
        public ClientBuilder ClientBuilder { get; }

        public ClientPeerBuilder(ClientBuilder builder)
        {
            ClientBuilder = builder;
        }

        public ClientPeerBuilder AddPeerListener<T>(T handler)
            where T : class, IPeerHandler
        {
            ClientBuilder.Configure(s =>
            {
                s.TryAddEnumerable(
                    ServiceDescriptor.Singleton<IPeerHandler, T>(p => handler));
            });
            return this;
        }

        public ClientPeerBuilder AddPeerListener<T>()
            where T : IPeerHandler
        {
            ClientBuilder.Configure(s =>
            {
                s.TryAddEnumerable(
                    ServiceDescriptor.Describe(
                        typeof(IPeerHandler),
                        typeof(T),
                        ServiceLifetime.Singleton));
            });
            return this;
        }
    }
}