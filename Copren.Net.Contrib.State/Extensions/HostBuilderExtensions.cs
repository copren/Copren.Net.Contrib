using Microsoft.Extensions.DependencyInjection;
using Copren.Net.Hosting.Hosting;

namespace Copren.Net.Contrib.State
{
    public static class HostBuilderExtensions
    {
        public static HostStateBuilder AddState(this HostBuilder self)
        {
            self.Configure(s =>
            {
                s.AddSingleton<HostStateManager>();
            });
            self.AddMiddleware<HostStateMiddleware>();

            return new HostStateBuilder(self);
        }
    }
}