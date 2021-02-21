using System;
using System.Threading.Tasks;

namespace Copren.Net.Contrib.Peers
{
    public interface IPeerHandler
    {
        Task OnPeerConnected(Guid peerId);
        Task OnPeerDisconnected(Guid peerId);
    }
}