// BidPlacedConsumer.cs
using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    // Define the consumer class which implements the IConsumer interface for the BidPlaced message
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        // Declare a private readonly field for the database context
        private readonly AuctionDbContext _dbContext;

        // Define the constructor which takes a database context as a parameter
        public BidPlacedConsumer(AuctionDbContext dbContext)
        {
            // Assign the database context to the private field
            _dbContext = dbContext;
        }

        // Define the Consume method which is called when a BidPlaced message is received
        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            // Log that the bid placed message is being consumed
            Console.WriteLine("--> Consuming placed bid");

            // Find the auction in the database that matches the AuctionId from the message
            var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

            // If there is no current high bid, or the bid status is "Accepted" and the bid amount is higher than the current high bid
            if (auction.CurrentHighBid == null
                || context.Message.BidStatus.Contains("Accepted")
                && context.Message.Amount > auction.CurrentHighBid)
            {
                // Update the auction's CurrentHighBid field with the bid amount from the message
                auction.CurrentHighBid = context.Message.Amount;

                // Save the changes to the database
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
