/*
 * This class is a consumer for the AuctionFinished event. It uses MassTransit, a message bus for .NET, 
 * to consume messages of type AuctionFinished. When a message is received, it sends a notification 
 * to all connected clients via a SignalR hub.
 */

using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        // The SignalR hub context used to communicate with clients
        private readonly IHubContext<NotificationHub> _hubContext;

        // Constructor that takes a hub context as a dependency
        public AuctionFinishedConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // The Consume method is called when a message of type AuctionFinished is received
        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            // Log that a message has been received
            Console.WriteLine("--> auction finished - message received");

            // Send a notification to all connected clients with the received message
            await _hubContext.Clients.All.SendAsync("AuctionFinished", context.Message);
        }
    }
}