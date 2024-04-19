// AuctionFinishedConsumer.cs
using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    // Define the consumer class which implements the IConsumer interface for the AuctionFinished message
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        // Declare a private readonly field for the database context
        private readonly AuctionDbContext _dbContext;

        // Define the constructor which takes a database context as a parameter
        public AuctionFinishedConsumer(AuctionDbContext dbContext)
        {
            // Assign the database context to the private field
            _dbContext = dbContext;
        }

        // Define the Consume method which is called when an AuctionFinished message is received
        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            // Log that the auction finished message is being consumed
            Console.WriteLine("--> Consuming finished auction");

            // Find the auction in the database that matches the AuctionId from the message
            var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

            // If the item was sold in the auction
            if (context.Message.ItemSold)
            {
                // Update the auction's Winner and SoldAmount fields with the data from the message
                auction.Winner = context.Message.Winner;
                auction.SoldAmount = context.Message.Amount;
            }

            // Update the auction's Status field based on whether the SoldAmount is greater than the ReservePrice
            if (auction.SoldAmount > auction.ReservePrice)
            {
                auction.Status = Status.Finished;
            }
            else
            {
                auction.Status = Status.ReserveNotMet;
            }

            // Save the changes to the database
            await _dbContext.SaveChangesAsync();
        }
    }
}