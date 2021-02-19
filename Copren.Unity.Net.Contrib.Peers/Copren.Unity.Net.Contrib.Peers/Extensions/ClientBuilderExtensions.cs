using Copren.Unity.Net.Core.Connection;
using Microsoft.Extensions.DependencyInjection;

namespace Copren.Unity.Net.Contrib.Peers
{
    public static class ClientBuilderExtensions
    {
        public static ClientPeerBuilder AddPeers(this ClientBuilder self)
        {
            self.AddMiddleware<ClientPeerMiddleware>();
            self.Configure(s => s.AddSingleton<PeerCollection>());
            return new ClientPeerBuilder(self);
        }
    }
}