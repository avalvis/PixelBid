//AuctionDbContext.cs is a class that defines the Auction database context. 
//This class inherits from DbContext and provides a representation of the Auction table in the database. 
//The class also overrides the OnModelCreating method to further configure the model by adding entities 
//for MassTransit features such as InboxState, OutboxState, and OutboxMessage.

using AuctionService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

// Define the namespace for the data layer
namespace AuctionService.Data
{
    // Define a class for the Auction database context that inherits from DbContext
    public class AuctionDbContext : DbContext
    {
        // Constructor that takes DbContextOptions and passes it to the base DbContext class
        public AuctionDbContext(DbContextOptions options) : base(options)
        {

        }

        // Define a DbSet for Auction entities. This represents the Auction table in the database.
        public DbSet<Auction> Auctions { get; set; }

        // Override the OnModelCreating method to further configure the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base OnModelCreating method
            base.OnModelCreating(modelBuilder);

            // Add the InboxState entity to the model. This is used by MassTransit for the inbox feature.
            modelBuilder.AddInboxStateEntity();

            // Add the OutboxState entity to the model. This is used by MassTransit for the outbox feature.
            modelBuilder.AddOutboxStateEntity();

            // Add the OutboxMessage entity to the model. This is used by MassTransit for the outbox feature.
            modelBuilder.AddOutboxMessageEntity();
        }
    }
}