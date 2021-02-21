using System.Threading.Tasks;
using Copren.Net.Core.Connection;

namespace Copren.Net.Contrib.State
{
    public interface IClientStateHandler
    {
        Task OnStateChanged(Client client, bool fromServer, object state);
    }
}