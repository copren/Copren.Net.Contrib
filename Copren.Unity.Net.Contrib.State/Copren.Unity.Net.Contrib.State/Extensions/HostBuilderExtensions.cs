using Microsoft.Extensions.DependencyInjection;
using Copren.Unity.Net.Hosting.Hosting;

namespace Copren.Unity.Net.Contrib.State
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