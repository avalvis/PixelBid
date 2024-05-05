/*
This class provides a client for the gRPC service.
It uses the Grpc.Net.Client package to create a gRPC channel and client.
The GetAuction method calls the GetAuction method of the gRPC service and returns the response as an Auction object.
If an error occurs when calling the gRPC service, the method logs the error and returns null.
*/

using AuctionService;
using BiddingService.Models;
using Grpc.Net.Client;

namespace BiddingService.Services
{
    public class GrpcAuctionClient
    {
        // Logger for logging information and errors
        private readonly ILogger<GrpcAuctionClient> _logger;
        // Configuration for accessing application settings
        private readonly IConfiguration _config;

        // Constructor that takes a logger and configuration as parameters
        public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        // Method to get auction details from the gRPC service
        public Auction GetAuction(string id)
        {
            // Log that the gRPC service is being called
            _logger.LogInformation("Calling the GRPC Service");

            // Create a gRPC channel for the address specified in the configuration
            var channel = GrpcChannel.ForAddress(_config["GrpcAuction"]);

            // Create a client for the gRPC service
            var client = new GrpcAuction.GrpcAuctionClient(channel);

            // Create a request with the specified auction ID
            var request = new GetAuctionRequest { Id = id };

            try
            {
                // Call the GetAuction method of the gRPC service and get the response
                var reply = client.GetAuction(request);

                // Create an Auction object from the response
                var auction = new Auction
                {
                    ID = reply.Auction.Id,
                    AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
                    Seller = reply.Auction.Seller,
                    ReservePrice = reply.Auction.ReservePrice
                };

                // Return the Auction object
                return auction;
            }
            catch (Exception ex)
            {
                // Log any errors that occur when calling the gRPC service
                _logger.LogError(ex, "Could not call GRPC Server");

                // Return null if an error occurs
                return null;
            }
        }
    }
}