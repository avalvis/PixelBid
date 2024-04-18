//AuctionsController.cs is a controller class for handling HTTP requests related to auctions.

using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    // AuctionsController is a controller class for handling HTTP requests related to auctions.
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        // Private fields for the database context, the mapper, and the publish endpoint.
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        // Constructor for the AuctionsController class.
        public AuctionsController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        // HTTP GET method to retrieve all auctions.
        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
        {
            // Query to retrieve all auctions, ordered by the title of the item.
            var query = _context.Auctions.OrderBy(x => x.Item.Title).AsQueryable();

            // If a date is provided, the query is updated to only include auctions updated after that date.
            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            }

            // Execute the query and map the results to a list of AuctionDto objects.
            return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        // HTTP GET method to retrieve a specific auction by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            // Query to retrieve the auction with the specified ID, including the related item.
            var auctions = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            // If the auction is not found, return a 404 Not Found response.
            if (auctions == null)
            {
                return NotFound();
            }

            // Map the auction to an AuctionDto object and return it.
            return _mapper.Map<AuctionDto>(auctions);
        }

        // HTTP POST method to create a new auction.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            // Map the auctionDto to an Auction object.
            var auction = _mapper.Map<Auction>(auctionDto);

            // Set the seller of the auction to the current user's username.
            auction.Seller = User.Identity.Name;

            // Add the auction to the database context.
            _context.Auctions.Add(auction);

            // Map the created auction to an AuctionDto object.
            var newAuction = _mapper.Map<AuctionDto>(auction);

            // Publish an AuctionCreated event with the created auction.
            await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

            // Save the changes to the database.
            var result = await _context.SaveChangesAsync() > 0;


            // If the save was not successful, return a 400 Bad Request response.
            if (!result)
            {
                return BadRequest("Failed to create auction");
            }

            // If the save was successful, return a 201 Created response with the created auction.
            return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, newAuction);
        }

        // HTTP PUT method to update an existing auction.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid Id, UpdateAuctionDto updateAuctionDto)
        {
            // Query to retrieve the auction with the specified ID, including the related item.
            var auction = await _context.Auctions.Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == Id);

            // If the auction is not found, return a 404 Not Found response.
            if (auction == null) return NotFound();

            if (auction.Seller != User.Identity.Name) return Forbid();

            // Update the auction with the data from the updateAuctionDto.
            auction.Item.Platform = updateAuctionDto.Platform ?? auction.Item.Platform;
            auction.Item.Title = updateAuctionDto.Title ?? auction.Item.Title;
            auction.Item.Genre = updateAuctionDto.Genre ?? auction.Item.Genre;
            auction.Item.PlayHours = updateAuctionDto.PlayHours ?? auction.Item.PlayHours;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            // Publish an AuctionUpdated event with the updated auction.
            await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));

            // Save the changes to the database.
            var result = await _context.SaveChangesAsync() > 0;

            // If the save was successful, return a 200 OK response.
            if (result) return Ok();

            // If the save was not successful, return a 400 Bad Request response.
            return BadRequest("Failed to update auction");
        }

        // HTTP DELETE method to delete an existing auction.
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            // Query to retrieve the auction with the specified ID.
            var auction = await _context.Auctions.FirstOrDefaultAsync(x => x.Id == id);

            // If the auction is not found, return a 404 Not Found response.
            if (auction == null) return NotFound();

            if (auction.Seller != User.Identity.Name) return Forbid();

            // Remove the auction from the database context.
            _context.Auctions.Remove(auction);

            // Publish an AuctionDeleted event with the ID of the deleted auction.
            await _publishEndpoint.Publish<AuctionDeleted>(new { Id = auction.Id.ToString() });

            // Save the changes to the database.
            var result = await _context.SaveChangesAsync() > 0;

            // If the save was not successful, return a 400 Bad Request response.
            if (!result) return BadRequest("Failed to delete auction");

            // If the save was successful, return a 200 OK response.
            return Ok();
        }
    }
}