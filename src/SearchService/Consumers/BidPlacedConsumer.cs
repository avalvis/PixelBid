// BidPlacedConsumer.cs
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    // Define the consumer class which implements the IConsumer interface for the BidPlaced message
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        // Define the Consume method which is called when a BidPlaced message is received
        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            // Log that the bid placed message is being consumed
            Console.WriteLine("--> Consuming bid placed");

            // Find the auction in the MongoDB database that matches the AuctionId from the message
            var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

            // If the bid status is "Accepted" and the bid amount is higher than the current high bid
            if (context.Message.BidStatus.Contains("Accepted")
                && context.Message.Amount > auction.CurrentHighBid)
            {
                // Update the auction's CurrentHighBid field with the bid amount from the message
                auction.CurrentHighBid = context.Message.Amount;

                // Save the changes to the MongoDB database
                await auction.SaveAsync();
            }
        }
    }
}