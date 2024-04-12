using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        // Create a paged search query for items
        var query = DB.PagedSearch<Item, Item>();

        // If a search term is provided, match items based on the search term
        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        // Sort the query based on the provided order by parameter
        query = searchParams.OrderBy switch
        {
            "title" => query.Sort(x => x.Ascending(y => y.Title)), // Sort by title if "title" is provided
            "new" => query.Sort(x => x.Descending(y => y.CreatedAt)), // Sort by creation date if "new" is provided
            _ => query.Sort(x => x.Ascending(y => y.AuctionEnd)) // Default sort is by auction end date
        };

        // Filter the query based on the provided filter by parameter
        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow), // Filter for finished auctions if "finished" is provided
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
            && x.AuctionEnd > DateTime.UtcNow), // Filter for auctions ending soon if "endingSoon" is provided
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow) // Default filter is for auctions ending in the future
        };

        // If a seller is provided, filter items based on the seller
        if (!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        // If a winner is provided, filter items based on the winner
        if (!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        // Set the page number for the query
        query.PageNumber(searchParams.PageNumber);
        // Set the page size for the query
        query.PageSize(searchParams.PageSize);

        // Execute the query asynchronously
        var result = await query.ExecuteAsync();

        // Return the results, page count, and total count in an Ok (200) response
        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}