// AuctionSvcHttpClient.cs is a service for making HTTP requests to the AuctionService.
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
    // This class is a service for making HTTP requests to the AuctionService
    public class AuctionSvcHttpClient
    {
        // These are private fields for the HTTP client and the configuration
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        // This is the constructor. It's called when an instance of the class is created.
        // The HTTP client and the configuration are injected into the service.
        public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        // This method gets items from the AuctionService
        public async Task<List<Item>> GetItemsForSearchDb()
        {
            // This creates a query to get the date of the last update from the Item collection
            var lastUpdated = await DB.Find<Item, string>()
                .Sort(x => x.Descending(x => x.UpdatedAt)) // Sort the items by the UpdatedAt field in descending order
                .Project(x => x.UpdatedAt.ToString()) // Project the result to get only the UpdatedAt field as a string
                .ExecuteFirstAsync(); // Execute the query and get the first result

            // This makes a GET request to the AuctionService to get items updated after the last update date
            // The result is deserialized into a list of Item objects
            return await _httpClient.GetFromJsonAsync<List<Item>>
            (_config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated);
        }
    }
}