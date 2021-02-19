using Copren.Unity.Net.Hosting.Hosting;

namespace Copren.Unity.Net.Contrib.Peers
{
    public static class HostBuilderExtensions
    {
        public static HostBuilder AddPeers(this HostBuilder self)
        {
            self.AddMiddleware<ServerPeerMiddleware>();
            return self;
        }
    }
}