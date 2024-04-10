using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Entities;

[Table("Items")]
public class Item
{
    public Guid Id { get; set; }
    public string Platform { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public int PlayHours { get; set; }
    public string ImageUrl { get; set; }

    // nav properties
    public Auction Auction { get; set; }
    public Guid AuctionId { get; set; }


}
