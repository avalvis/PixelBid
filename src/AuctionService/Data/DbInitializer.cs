
using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class DbInitializer
{
    public static void initDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetRequiredService<AuctionDbContext>());
    }

    private static void SeedData(AuctionDbContext context)
    {
        context.Database.Migrate();

        if (context.Auctions.Any())
        {
            Console.WriteLine("Data already exists, no need to seed!");
            return;
        }

        var auctions = new List<Auction>
        {
            // 1 The Legend of Zelda: Breath of the Wild
            new Auction
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Live,
                ReservePrice = 45,
                Seller = "tony",
                AuctionEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Platform = "Nintendo Switch",
                    Title = "The Legend of Zelda: Breath of the Wild",
                    Genre = "Adventure",
                    PlayHours = 50,
                    Year = 2017,
                    ImageUrl = "https://i.ibb.co/Hn2whWv/zelda-botw.jpg"
                }
            },
            // 2 God of War
            new Auction
            {
                Id = Guid.Parse("c8c3ec17-01bf-49db-82aa-1ef80b833a9f"),
                Status = Status.Live,
                ReservePrice = 30,
                Seller = "maria",
                AuctionEnd = DateTime.UtcNow.AddDays(60),
                Item = new Item
                {
                    Platform = "PS4",
                    Title = "God of War",
                    Genre = "Action",
                    PlayHours = 30,
                    Year = 2018,
                    ImageUrl = "https://i.ibb.co/rHRKddp/god-of-war-ps4.jpg"
                }
            },
            // 3 Cyberpunk 2077
            new Auction
            {
                Id = Guid.Parse("bbab4d5a-8565-48b1-9450-5ac2a5c4a654"),
                Status = Status.Live,
                Seller = "tony",
                AuctionEnd = DateTime.UtcNow.AddDays(4),
                Item = new Item
                {
                    Platform = "PC",
                    Title = "Cyberpunk 2077",
                    Genre = "RPG",
                    PlayHours = 100,
                    Year = 2020,
                    ImageUrl = "https://i.ibb.co/vYdM47n/cyberpunk2077.jpg"
                }
            },
            // 4 Halo Infinite
            new Auction
            {
                Id = Guid.Parse("155225c1-4448-4066-9886-6786536e05ea"),
                Status = Status.ReserveNotMet,
                ReservePrice = 40,
                Seller = "john",
                AuctionEnd = DateTime.UtcNow.AddDays(-10),
                Item = new Item
                {
                    Platform = "Xbox Series X",
                    Title = "Halo Infinite",
                    Genre = "Shooter",
                    PlayHours = 45,
                    Year = 2021,
                    ImageUrl = "https://i.ibb.co/Kx1gTWm/halo-infinite.jpg"
                }
            },
            // 5 Animal Crossing: New Horizons
            new Auction
            {
                Id = Guid.Parse("466e4744-4dc5-4987-aae0-b621acfc5e39"),
                Status = Status.Live,
                ReservePrice = 50,
                Seller = "maria",
                AuctionEnd = DateTime.UtcNow.AddDays(30),
                Item = new Item
                {
                    Platform = "Nintendo Switch",
                    Title = "Animal Crossing: New Horizons",
                    Genre = "Simulation",
                    PlayHours = 150,
                    Year = 2020,
                    ImageUrl = "https://i.ibb.co/3d2GnNy/nintendo-switch-animal-crossin.jpg"
                }
            },
            // 6 Spider-Man: Miles Morales
            new Auction
            {
                Id = Guid.Parse("dc1e4071-d19d-459b-b848-b5c3cd3d151f"),
                Status = Status.Live,
                ReservePrice = 35,
                Seller = "tony",
                AuctionEnd = DateTime.UtcNow.AddDays(45),
                Item = new Item
                {
                    Platform = "PS5",
                    Title = "Spider-Man: Miles Morales",
                    Genre = "Action",
                    PlayHours = 25,
                    Year = 2020,
                    ImageUrl = "https://i.ibb.co/FKqHypW/spiderman-miles.jpg"
                }
            },
            // 7 Assassin’s Creed Valhalla
            new Auction
            {
                Id = Guid.Parse("47111973-d176-4feb-848d-0ea22641c31a"),
                Status = Status.Live,
                ReservePrice = 40,
                Seller = "maria",
                AuctionEnd = DateTime.UtcNow.AddDays(13),
                Item = new Item
                {
                    Platform = "PS5",
                    Title = "Assassin’s Creed Valhalla",
                    Genre = "RPG",
                    PlayHours = 60,
                    Year = 2020,
                    ImageUrl = "https://i.ibb.co/hsF0H26/ac-valhalla.jpg"
                }
            },
            // 8 Final Fantasy VII Remake
            new Auction
            {
                Id = Guid.Parse("6a5011a1-fe1f-47df-9a32-b5346b289391"),
                Status = Status.Live,
                Seller = "tony",
                AuctionEnd = DateTime.UtcNow.AddDays(19),
                Item = new Item
                {
                    Platform = "PS4",
                    Title = "Final Fantasy VII Remake",
                    Genre = "RPG",
                    PlayHours = 40,
                    Year = 2020,
                    ImageUrl = "https://i.ibb.co/CQJ50f1/final-fantasy-7-ps4.jpg"
                }
            },
            // 9 The Witcher 3: Wild Hunt
            new Auction
            {
                Id = Guid.Parse("40490065-dac7-46b6-acc4-df507e0d6570"),
                Status = Status.Live,
                ReservePrice = 25,
                Seller = "john",
                AuctionEnd = DateTime.UtcNow.AddDays(20),
                Item = new Item
                {
                    Platform = "PS4",
                    Title = "The Witcher 3: Wild Hunt",
                    Genre = "RPG",
                    PlayHours = 100,
                    Year = 2015,
                    ImageUrl = "https://i.ibb.co/NWN0FZq/witcher-ps4.jpg"
                }
            },
            // 10 Elden Ring
            new Auction
            {
                Id = Guid.Parse("3659ac24-29dd-407a-81f5-ecfe6f924b9b"),
                Status = Status.Live,
                ReservePrice = 50,
                Seller = "tony",
                AuctionEnd = DateTime.UtcNow.AddDays(48),
                Item = new Item
                {
                    Platform = "PS5",
                    Title = "Elden Ring",
                    Genre = "RPG",
                    PlayHours = 80,
                    Year = 2022,
                    ImageUrl = "https://i.ibb.co/K6P03Kj/elden-ring-ps5.jpg"
                }
            }

        };

        context.AddRange(auctions);

        context.SaveChanges();

    }
}
