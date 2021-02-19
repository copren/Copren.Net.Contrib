using Copren.Unity.Net.Contrib.State;

namespace Copren.Unity.Net.Contrib.Peers.State
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