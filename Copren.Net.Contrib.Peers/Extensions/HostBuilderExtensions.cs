using Copren.Net.Hosting.Hosting;

namespace Copren.Net.Contrib.Peers
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