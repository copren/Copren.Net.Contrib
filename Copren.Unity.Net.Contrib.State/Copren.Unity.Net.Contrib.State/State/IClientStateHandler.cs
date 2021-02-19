using System.Threading.Tasks;
using Copren.Unity.Net.Core.Connection;

namespace Copren.Unity.Net.Contrib.State
{
    public interface IClientStateHandler
    {
        Task OnStateChanged(Client client, bool fromServer, object state);
    }
}