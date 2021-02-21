using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Copren.Net.Contrib.Peers.Messages;
using Copren.Net.Domain;
using Copren.Net.Domain.Messaging.Messages;
using Serilog;
using Copren.Net.Hosting.Context;
using Copren.Net.Hosting.Messaging;
using Copren.Net.Hosting.Middleware;

namespace Copren.Net.Contrib.Peers
{
    public class ServerPeerMiddleware : IMiddleware
    {
        private readonly MessageCenter _messageCenter;
        private readonly ILogger _logger;

        public ServerPeerMiddleware(MessageCenter messageCenter, ILogger logger)
        {
            _messageCenter = messageCenter;
            _logger = logger.ForContext<ServerPeerMiddleware>();
        }

        public async Task OnClientConnected(HostContext context, Client client, Func<Task> next)
        {
            _logger.Information("Client \"{ClientId}\" connected", client.ClientId);
            foreach (var c in context.Host.Clients)
            {
                // Skip the connected client
                if (c.ClientId == client.ClientId) continue;

                // Send the connected client as a peer to the client
                var clientUri = client.Uris[ProtocolType.Udp];
                _logger.Information("Sending peer identity message to {ClientId:s} -> {Uri}", client.ClientId, clientUri);
                await _messageCenter.SendMessage(
                    clientUri,
                    new PeerIdentifyMessage(c));

                // Send the peer to the connected client
                var cUri = c.Uris[ProtocolType.Udp];
                _logger.Information("Sending peer identity message to {ClientId:s} -> {Uri}", c.ClientId, cUri);
                await _messageCenter.SendMessage(
                    cUri,
                    new PeerIdentifyMessage(client));
            }

            await next();
        }

        public Task OnClientDisconnected(HostContext context, Client client, Func<Task> next)
        {
            return next();
        }

        public Task OnMessage(HostContext context, Message message, Func<Task> next)
        {
            return next();
        }
    }
}