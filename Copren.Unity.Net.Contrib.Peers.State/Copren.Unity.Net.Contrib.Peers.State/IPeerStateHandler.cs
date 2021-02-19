using System;
using System.Threading.Tasks;
using Copren.Unity.Net.Core.Connection;

namespace Copren.Unity.Net.Contrib.Peers.State
{
    public interface IPeerStateHandler
    {
        Task OnPeerStateChanged(Client client, Guid peerId, object state);
    }
}