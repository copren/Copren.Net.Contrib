using Copren.Net.Contrib.State;

namespace Copren.Net.Contrib.Peers.State
{
    public static class HostStateBuilderExtensions
    {
        public static HostStateBuilder UpdateClients(this HostStateBuilder self)
        {
            self.AddStateListener<HostStateHandler>();
            return self;
        }
    }
}