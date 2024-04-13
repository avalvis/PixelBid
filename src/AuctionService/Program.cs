// Import necessary namespaces
using AuctionService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

// Create a new web application builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Add MVC Controllers as services
builder.Services.AddControllers();

// Add the AuctionDbContext to the services and configure it to use PostgreSQL
// The connection string is retrieved from the application's configuration
builder.Services.AddDbContext<AuctionDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper to the services and scan the current domain's assemblies for AutoMapper profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add MassTransit to the services and configure it
builder.Services.AddMassTransit(x =>
{
    // Configure MassTransit to use RabbitMQ as the message broker
    x.UsingRabbitMq((context, cfg) =>
    {
        // Configure the RabbitMQ endpoints based on the registered services
        // 'context' provides access to the application's services
        // 'cfg' is used to configure the interaction with RabbitMQ
        cfg.Host("192.168.1.239");
        cfg.ConfigureEndpoints(context);
    });
});

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline

// Add authorization middleware to the pipeline
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Initialize the database
// If an error occurs, catch the exception and write it to the console
try
{
    DbInitializer.initDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

// Run the application
app.Run();