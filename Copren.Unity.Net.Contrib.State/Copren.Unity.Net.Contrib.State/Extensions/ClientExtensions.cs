using System.Threading.Tasks;
using Copren.Unity.Net.Core.Connection;
using Microsoft.Extensions.DependencyInjection;

namespace Copren.Unity.Net.Contrib.State
{
    public static class ClientExtensions
    {
        public static ClientStateManager State(this Client self)
        {
            return self.ServiceProvider.GetRequiredService<ClientStateManager>();
        }

        public static Task UpdateStateAsync(this Client self, object state)
        {
            return self.State().UpdateAsync(state);
        }
    }
}