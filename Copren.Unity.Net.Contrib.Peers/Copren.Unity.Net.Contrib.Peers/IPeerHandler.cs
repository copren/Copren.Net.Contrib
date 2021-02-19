using System;
using System.Threading.Tasks;

namespace Copren.Unity.Net.Contrib.Peers
{
    public interface IPeerHandler
    {
        Task OnPeerConnected(Guid peerId);
        Task OnPeerDisconnected(Guid peerId);
    }
}