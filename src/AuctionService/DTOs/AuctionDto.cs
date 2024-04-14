// AuctionDto.cs is a DTO class that represents an auction. 
// It contains properties that map to the fields of an auction entity, such as the ID, reserve price, seller, winner, sold amount, 
// current high bid, created at, updated at, auction end, status, platform, title, year, genre, play hours, and image URL. 
// This class is used to transfer data between the controller and the client in a structured format. 
// The DTO class helps to decouple the internal representation of an auction from the external representation, allowing for flexibility in the data transfer process.
namespace AuctionService.DTOs;

public class AuctionDto
{
    public Guid Id { get; set; }
    public int ReservePrice { get; set; }
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int SoldAmount { get; set; }
    public int CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime AuctionEnd { get; set; }
    public string Status { get; set; }
    public string Platform { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public int PlayHours { get; set; }
    public string ImageUrl { get; set; }

}
