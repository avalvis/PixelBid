/*
 * This class is a consumer for the AuctionCreated event. It uses MassTransit, a message bus for .NET, 
 * to consume messages of type AuctionCreated. When a message is received, it sends a notification 
 * to all connected clients via a SignalR hub.
 */

using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        // The SignalR hub context used to communicate with clients
        private readonly IHubContext<NotificationHub> _hubContext;

        // Constructor that takes a hub context as a dependency
        public AuctionCreatedConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // The Consume method is called when a message of type AuctionCreated is received
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            // Log that a message has been received
            Console.WriteLine("--> auction created - message received");

            // Send a notification to all connected clients with the received message
            await _hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);
        }
    }
}