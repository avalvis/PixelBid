// Import necessary namespaces
using SearchService.Data;
using SearchService.Services;
using SearchService.Utilities;

// Create a new web application builder with the provided command line arguments
var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Add the MVC controllers service to the application
builder.Services.AddControllers();

// Add an HTTP client for the AuctionSvcHttpClient service with a Polly policy for handling HTTP request retries
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(PollyUtility.GetPolicy());

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline

// Add authorization middleware to the pipeline
app.UseAuthorization();

// Map the MVC controllers
app.MapControllers();

// Register a callback to be invoked when the application has started
// This callback initializes the database
app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        // Try to initialize the database
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        // If an error occurs during database initialization, log the error
        Console.WriteLine("Error initializing the database");
        Console.WriteLine(e.Message);
    }
});

// Run the application
app.Run();