using System;
using System.Threading.Tasks;
using Copren.Net.Core.Connection;

namespace Copren.Net.Contrib.Peers.State
{
    public interface IPeerStateHandler
    {
        Task OnPeerStateChanged(Client client, Guid peerId, object state);
    }
}