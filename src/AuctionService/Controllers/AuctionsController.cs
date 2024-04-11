using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuctionService.Controllers
{
    // This class is a controller for handling HTTP requests related to auctions.
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        // These are private fields for the database context and the mapper.
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;

        // This is the constructor. It's called when an instance of the class is created.
        public AuctionsController(AuctionDbContext context, IMapper mapper)
        {
            // The database context and the mapper are injected into the controller.
            _context = context;
            _mapper = mapper;
        }

        // This method handles GET requests to get all auctions.
        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
        {
            // This creates a query to get all auctions, ordered by the title of the item.
            var query = _context.Auctions.OrderBy(x => x.Item.Title).AsQueryable();

            // If a date is provided, the query is updated to only include auctions updated after that date.
            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            }

            // The query is executed, and the results are mapped to a list of AuctionDto objects.
            return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        // This method handles GET requests to get a specific auction by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            // This creates a query to get the auction with the specified ID, including the related item.
            var auctions = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            // If the auction is not found, a 404 Not Found response is returned.
            if (auctions == null)
            {
                return NotFound();
            }

            // The auction is mapped to an AuctionDto object and returned.
            return _mapper.Map<AuctionDto>(auctions);
        }

        // This method handles POST requests to create a new auction.
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            // The auctionDto is mapped to an Auction object.
            var auction = _mapper.Map<Auction>(auctionDto);
            // The seller is set to "test" (this should be replaced with the current user).
            auction.Seller = "test";

            // The auction is added to the database context.
            _context.Auctions.Add(auction);

            // The changes are saved to the database.
            var result = await _context.SaveChangesAsync() > 0;

            // If the save was not successful, a 400 Bad Request response is returned.
            if (!result)
            {
                return BadRequest("Failed to create auction");
            }

            // If the save was successful, a 201 Created response is returned, with the created auction.
            return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, _mapper.Map<AuctionDto>(auction));
        }

        // This method handles PUT requests to update an existing auction.
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid Id, UpdateAuctionDto updateAuctionDto)
        {
            // This creates a query to get the auction with the specified ID, including the related item.
            var auction = await _context.Auctions.Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == Id);

            // If the auction is not found, a 404 Not Found response is returned.
            if (auction == null) return NotFound();

            // The auction is updated with the data from the updateAuctionDto.
            auction.Item.Platform = updateAuctionDto.Platform ?? auction.Item.Platform;
            auction.Item.Title = updateAuctionDto.Title ?? auction.Item.Title;
            auction.Item.Genre = updateAuctionDto.Genre ?? auction.Item.Genre;
            auction.Item.PlayHours = updateAuctionDto.PlayHours ?? auction.Item.PlayHours;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            // The changes are saved to the database.
            var result = await _context.SaveChangesAsync() > 0;

            // If the save was successful, a 200 OK response is returned.
            if (result) return Ok();

            // If the save was not successful, a 400 Bad Request response is returned.
            return BadRequest("Failed to update auction");
        }

        // This method handles DELETE requests to delete an existing auction.
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            // This creates a query to get the auction with the specified ID.
            var auction = await _context.Auctions.FirstOrDefaultAsync(x => x.Id == id);

            // If the auction is not found, a 404 Not Found response is returned.
            if (auction == null) return NotFound();

            // The auction is removed from the database context.
            _context.Auctions.Remove(auction);

            // The changes are saved to the database.
            var result = await _context.SaveChangesAsync() > 0;

            // If the save was not successful, a 400 Bad Request response is returned.
            if (!result) return BadRequest("Failed to delete auction");

            // If the save was successful, a 200 OK response is returned.
            return Ok();
        }
    }
}