//AuctionDeletedConsumer.cs is a consumer class that consumes messages of type AuctionDeleted.
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    // This class is a consumer for the AuctionDeleted message.
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        // This method is called when an AuctionDeleted message is consumed.
        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            // Log the ID of the auction that was deleted.
            Console.WriteLine("--> Consuming AuctionDeleted: " + context.Message.Id);

            // Delete the corresponding item from the database.
            var result = await DB.DeleteAsync<Item>(context.Message.Id);

            // If the delete operation was not acknowledged by the database, throw an exception.
            if (!result.IsAcknowledged)
                throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction");
        }
    }
}