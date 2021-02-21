using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Copren.Net.Hosting.Hosting;

namespace Copren.Net.Contrib.State
{
    public class HostStateBuilder
    {
        public HostBuilder HostBuilder { get; }

        public HostStateBuilder(HostBuilder builder)
        {
            HostBuilder = builder;
        }

        public HostStateBuilder AddStateListener<T>(T handler)
            where T : class, IHostStateHandler
        {
            HostBuilder.Configure(s =>
            {
                s.TryAddEnumerable(
                    ServiceDescriptor.Singleton<IHostStateHandler, T>(p => handler));
            });
            return this;
        }

        public HostStateBuilder AddStateListener<T>()
            where T : IHostStateHandler
        {
            HostBuilder.Configure(s =>
            {
                s.TryAddEnumerable(
                    ServiceDescriptor.Describe(
                        typeof(IHostStateHandler),
                        typeof(T),
                        ServiceLifetime.Singleton));
            });
            return this;
        }
    }
}