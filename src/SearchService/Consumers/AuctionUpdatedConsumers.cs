// AuctionUpdatedConsumer.cs is a consumer class that consumes messages of type AuctionUpdated. 
// It updates the item in the database with the new values provided in the message. 
// The consumer uses AutoMapper to map the message to an Item entity and then updates the item in the database using MongoDB.Entities.
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    // This class is a consumer for the AuctionUpdated message.
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        // AutoMapper is used to map properties from one object to another.
        private readonly IMapper _mapper;

        // The constructor takes an IMapper instance, which will be provided by dependency injection.
        public AuctionUpdatedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }

        // This method is called when an AuctionUpdated message is consumed.
        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            // Log the ID of the auction that was updated.
            Console.WriteLine("--> Consuming auction updated: " + context.Message.Id);

            // Map the AuctionUpdated message to an Item object.
            var item = _mapper.Map<Item>(context.Message);

            // Update the corresponding item in the database.
            var result = await DB.Update<Item>()
                // Find the item with the same ID as the auction.
                .Match(a => a.ID == context.Message.Id)
                // Update only the specified properties of the item.
                .ModifyOnly(x => new
                {
                    x.Genre,
                    x.Platform,
                    x.Title,
                    x.Year,
                    x.PlayHours
                }, item)
                // Execute the update operation.
                .ExecuteAsync();

            // If the update operation was not acknowledged by the database, throw an exception.
            if (!result.IsAcknowledged)
                throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
        }
    }
}
