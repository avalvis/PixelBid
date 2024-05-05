/*
This class provides the implementation for the gRPC service defined in the GrpcAuctionBase class.
It uses the AuctionDbContext to interact with the database and retrieve auction details.
*/

using AuctionService.Data;
using Grpc.Core;

namespace AuctionService.Services
{
    public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
    {
        // The database context used to interact with the database
        private readonly AuctionDbContext _dbContext;

        // Constructor that takes the database context as a parameter
        public GrpcAuctionService(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Override the GetAuction method defined in the GrpcAuctionBase class
        public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request, ServerCallContext context)
        {
            // Log that a gRPC request has been received
            Console.WriteLine("==> Received a Grpc request for auction");

            // Find the auction with the requested ID in the database
            var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(request.Id))
                // If the auction is not found, throw an exception
                ?? throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

            // Create a response with the auction details
            var response = new GrpcAuctionResponse
            {
                Auction = new GrpcAuctionModel
                {
                    AuctionEnd = auction.AuctionEnd.ToString(),
                    Id = auction.Id.ToString(),
                    ReservePrice = auction.ReservePrice,
                    Seller = auction.Seller
                }
            };

            // Return the response
            return response;
        }
    }
}