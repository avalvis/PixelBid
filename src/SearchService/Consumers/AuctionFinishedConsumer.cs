//AuctionFinishedConsumer.cs
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    // Define the consumer class which implements the IConsumer interface for the AuctionFinished message
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        // Define the Consume method which is called when an AuctionFinished message is received
        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            // Find the auction in the MongoDB database that matches the AuctionId from the message
            var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

            // If the item was sold in the auction
            if (context.Message.ItemSold)
            {
                // Update the auction's Winner and SoldAmount fields with the data from the message
                auction.Winner = context.Message.Winner;
                auction.SoldAmount = (int)context.Message.Amount;
            }

            // Set the auction's Status field to "Finished"
            auction.Status = "Finished";

            // Save the changes to the MongoDB database
            await auction.SaveAsync();
        }
    }
}