/*
 * This class is a consumer for the BidPlaced event. It uses MassTransit, a message bus for .NET, 
 * to consume messages of type BidPlaced. When a message is received, it sends a notification 
 * to all connected clients via a SignalR hub.
 */

using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        // The SignalR hub context used to communicate with clients
        private readonly IHubContext<NotificationHub> _hubContext;

        // Constructor that takes a hub context as a dependency
        public BidPlacedConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // The Consume method is called when a message of type BidPlaced is received
        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            // Log that a message has been received
            Console.WriteLine("--> bid placed message received");

            // Send a notification to all connected clients with the received message
            await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
        }
    }
}