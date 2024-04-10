namespace AuctionService.DTOs;

public class UpdateAuctionDto
{
    public string Platform { get; set; }
    public string Title { get; set; }
    public int? Year { get; set; }
    public string Genre { get; set; }
    public int? PlayHours { get; set; }
}
