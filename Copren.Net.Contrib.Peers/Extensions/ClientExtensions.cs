using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Copren.Net.Core.Connection;
using Copren.Net.Domain.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Copren.Net.Contrib.Peers
{
    public static class ClientExtensions
    {
        public static Task BroadcastMessageAsync<T>(this Client self, T message)
            where T : Message
        {
            var peerCollection = self.ServiceProvider.GetRequiredService<PeerCollection>();
            var tasks = new List<Task>();
            tasks.Add(self.SendServerMessageAsync(message));
            foreach (var peer in peerCollection)
            {
                tasks.Add(self.SendPeerMessageAsync(peer, message));
            }
            return Task.WhenAll(tasks);
        }

        public static async Task BroadcastPeerMessageAsync<T>(this Client self, Func<Peer, Task<T>> factory)
            where T : Message
        {
            var peerCollection = self.ServiceProvider.GetRequiredService<PeerCollection>();
            var tasks = new List<Task>();
            foreach (var peer in peerCollection)
            {
                tasks.Add(self.SendPeerMessageAsync(peer, await factory.Invoke(peer)));
            }
            await Task.WhenAll(tasks);
        }

        public static Task BroadcastPeerMessageAsync<T>(this Client self, T message)
            where T : Message
        {
            var peerCollection = self.ServiceProvider.GetRequiredService<PeerCollection>();
            var tasks = new List<Task>();
            foreach (var peer in peerCollection)
            {
                tasks.Add(self.SendPeerMessageAsync(peer, message));
            }
            return Task.WhenAll(tasks);
        }

        public static Task SendPeerMessageAsync<T>(this Client self, Peer peer, T message)
            where T : Message
        {
            return self.SendMessageAsync(peer.Uri, message);
        }
    }
}