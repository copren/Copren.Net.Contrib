using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Copren.Unity.Net.Core.Connection;
using Copren.Unity.Net.Domain.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Copren.Unity.Net.Hosting;
using Copren.Unity.Net.Hosting.Hosting;

namespace Copren.Unity.Net.Contrib.Peers
{
    public static class HostExtensions
    {
        public static Task BroadcastMessageAsync<T>(this Host self, T message)
            where T : Message
        {
            var clientCollection = self.ServiceProvider.GetRequiredService<ClientCollection>();
            var tasks = new List<Task>();
            foreach (var client in clientCollection)
            {
                tasks.Add(self.SendClientMessageAsync(client, message));
            }
            return Task.WhenAll(tasks);
        }

        public static Task BroadcastClientMessageAsync<T>(this Host self, T message, HashSet<Client> except = null)
            where T : Message
        {
            return self.BroadcastClientMessageAsync(message, new HashSet<Guid>(except.Select(c => c.Id)));
        }

        public static Task BroadcastClientMessageAsync<T>(this Host self, T message, HashSet<Guid> except = null)
            where T : Message
        {
            var clientCollection = self.ServiceProvider.GetRequiredService<ClientCollection>();
            var tasks = new List<Task>();
            foreach (var client in clientCollection)
            {
                // Skip any excepted clients
                if (except?.Contains(client.ClientId) == true) continue;

                tasks.Add(self.SendClientMessageAsync(client, message));
            }
            return Task.WhenAll(tasks);
        }
    }
}