using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data
{
    public class DbInitializer
    {
        // This method initializes the database and populates it with data from the AuctionService
        public static async Task InitDb(WebApplication app)
        {
            // Initialize the MongoDB database with the provided connection string
            await DB.InitAsync("SearchDb", MongoClientSettings
                .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

            // Create text indexes on the Platform, Title, and Genre fields of the Item collection
            // This will improve search performance on these fields
            await DB.Index<Item>()
                .Key(x => x.Platform, KeyType.Text)
                .Key(x => x.Title, KeyType.Text)
                .Key(x => x.Genre, KeyType.Text)
                .CreateAsync();

            // Get the count of documents in the Item collection
            var count = await DB.CountAsync<Item>();

            // Create a new scope for dependency injection
            using var scope = app.Services.CreateScope();

            // Get an instance of the AuctionSvcHttpClient service
            // This service is used to make HTTP requests to the AuctionService
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

            // Get items from the AuctionService
            var items = await httpClient.GetItemsForSearchDb();

            // Log the count of items retrieved from the AuctionService
            Console.WriteLine($"Items from AuctionService: {items.Count}");

            // If there are any items, save them to the MongoDB database
            if (items.Count > 0)
            {
                await DB.SaveAsync(items);
            }
        }
    }
}