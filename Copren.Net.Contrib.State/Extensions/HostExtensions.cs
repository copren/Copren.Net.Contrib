using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Copren.Net.Hosting.Hosting;

namespace Copren.Net.Contrib.State.Extensions
{
    public static class HostExtensions
    {
        public static HostStateManager State(this Host self)
        {
            return self.ServiceProvider.GetRequiredService<HostStateManager>();
        }

        public static Task UpdateStateAsync(this Host self, Guid clientId, object state)
        {
            return self.State().UpdateAsync(clientId, state);
        }
    }
}