using System;
using System.Threading.Tasks;
using Copren.Unity.Net.Hosting.Hosting;

namespace Copren.Unity.Net.Contrib.State
{
    public interface IHostStateHandler
    {
        Task OnStateChanged(Host host, bool fromServer, Guid clientId, object state);
    }
}