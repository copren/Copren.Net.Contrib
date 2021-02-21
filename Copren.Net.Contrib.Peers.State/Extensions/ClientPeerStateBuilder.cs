using Copren.Net.Contrib.State;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Copren.Net.Contrib.Peers.State
{
    public class ClientPeerStateBuilder
    {
        public ClientStateBuilder ClientStateBuilder { get; }

        public ClientPeerStateBuilder(ClientStateBuilder clientStateBuilder)
        {
            ClientStateBuilder = clientStateBuilder;
        }

        public ClientPeerStateBuilder AddStateListener<T>(T handler)
            where T : class, IPeerStateHandler
        {
            ClientStateBuilder.ClientBuilder.Configure(s =>
            {
                s.TryAddEnumerable(
                    ServiceDescriptor.Singleton<IPeerStateHandler, T>(p => handler));
            });
            return this;
        }

        public ClientPeerStateBuilder AddStateListener<T>()
            where T : IPeerStateHandler
        {
            ClientStateBuilder.ClientBuilder.Configure(s =>
            {
                s.TryAddEnumerable(
                    ServiceDescriptor.Describe(
                        typeof(IPeerStateHandler),
                        typeof(T),
                        ServiceLifetime.Singleton));
            });
            return this;
        }
    }
}