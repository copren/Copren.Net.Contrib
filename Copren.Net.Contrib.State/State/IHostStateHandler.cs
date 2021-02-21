using System;
using System.Threading.Tasks;
using Copren.Net.Hosting.Hosting;

namespace Copren.Net.Contrib.State
{
    public interface IHostStateHandler
    {
        Task OnStateChanged(Host host, bool fromServer, Guid clientId, object state);
    }
}