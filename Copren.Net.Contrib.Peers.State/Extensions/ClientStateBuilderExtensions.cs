using Copren.Net.Contrib.State;
using Copren.Net.Core.Connection;
using Microsoft.Extensions.DependencyInjection;

namespace Copren.Net.Contrib.Peers.State
{
    public static class ClientStateBuilderExtensions
    {
        public static ClientPeerStateBuilder UpdatePeers(this ClientStateBuilder self)
        {
            self.AddStateListener<ClientStateHandler>();
            self.ClientBuilder.Configure(s => s.AddSingleton<PeerStateManager>());
            self.ClientBuilder.AddMiddleware<PeerStateMiddleware>();
            return new ClientPeerStateBuilder(self);
        }
    }
}