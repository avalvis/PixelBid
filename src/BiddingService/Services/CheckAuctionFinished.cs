/*
This class is a background service that periodically checks for auctions that have finished.
An auction is considered finished if its end time has passed and it hasn't been marked as finished yet.
For each finished auction, it marks it as finished, finds the winning bid (the highest accepted bid),
and publishes an AuctionFinished message.
*/

using BiddingService.Models;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService.Services
{
    public class CheckAuctionFinished : BackgroundService
    {
        private readonly ILogger<CheckAuctionFinished> _logger;
        private readonly IServiceProvider _services;

        // Constructor
        public CheckAuctionFinished(ILogger<CheckAuctionFinished> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        // This method is called when the background service starts
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting check for finished auctions");

            // Register a callback to be called when the service is stopping
            stoppingToken.Register(() => _logger.LogInformation("==> Auction check is stopping"));

            // Keep checking for finished auctions until the service is stopped
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAuctions(stoppingToken);

                // Wait for 5 seconds before checking again
                await Task.Delay(5000, stoppingToken);
            }
        }

        // This method checks for finished auctions and handles them
        private async Task CheckAuctions(CancellationToken stoppingToken)
        {
            // Find auctions that have finished but haven't been marked as finished yet
            var finishedAuctions = await DB.Find<Auction>()
                .Match(x => x.AuctionEnd <= DateTime.UtcNow)
                .Match(x => !x.Finished)
                .ExecuteAsync(stoppingToken);

            // If there are no finished auctions, return
            if (finishedAuctions.Count == 0) return;

            _logger.LogInformation("==> Found {count} auctions that have completed", finishedAuctions.Count);

            // Create a new scope for dependency injection
            using var scope = _services.CreateScope();
            // Get the IPublishEndpoint service
            var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            // Handle each finished auction
            foreach (var auction in finishedAuctions)
            {
                // Mark the auction as finished
                auction.Finished = true;
                await auction.SaveAsync(null, stoppingToken);

                // Find the winning bid for the auction
                var winningBid = await DB.Find<Bid>()
                    .Match(a => a.AuctionId == auction.ID)
                    .Match(b => b.BidStatus == BidStatus.Accepted)
                    .Sort(x => x.Descending(s => s.Amount))
                    .ExecuteFirstAsync(stoppingToken);

                // Publish an AuctionFinished message
                await endpoint.Publish(new AuctionFinished
                {
                    ItemSold = winningBid != null,
                    AuctionId = auction.ID,
                    Winner = winningBid?.Bidder,
                    Amount = winningBid?.Amount,
                    Seller = auction.Seller
                }, stoppingToken);
            }
        }
    }
}