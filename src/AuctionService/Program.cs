// Program.cs
// Import necessary namespaces
using AuctionService.Consumers;
using AuctionService.Data;
using AuctionService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    // Add the outbox to the service bus
    // Configure the outbox to use the specified DbContext
    x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
    {   // Configure the outbox to use a delay of 10 seconds for query retries
        o.QueryDelay = TimeSpan.FromSeconds(10);
        // Configure the outbox to use PostgreSQL
        o.UsePostgres();
        // Configure the outbox to use the specified DbContext
        o.UseBusOutbox();
    });
    // Add the consumers to the MassTransit configuration
    x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();

    // Configure the endpoint name formatter to use kebab case
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

    // Configure MassTransit to use RabbitMQ as the message broker
    x.UsingRabbitMq((context, cfg) =>
    {
        // Configure the RabbitMQ endpoints based on the registered services
        // 'context' provides access to the application's services

        // Configure the host to use the specified RabbitMQ host, username, and password
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
        {
            // Set the username and password to the values specified in the configuration
            h.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            h.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ConfigureEndpoints(context);
    });
});


// Add authentication services to the DI container, specifying JWT Bearer as the default scheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Set the authority to the URL of the identity service, which is used to validate the tokens
        options.Authority = builder.Configuration["IdentityServiceUrl"];

        // Disable requiring HTTPS for metadata retrieval. In production, this should be enabled for security
        options.RequireHttpsMetadata = false;

        // Disable audience validation. In production, this should be enabled and set to the valid audiences
        options.TokenValidationParameters.ValidateAudience = false;

        // Set the claim type for the user's name in the JWT token to "username"
        options.TokenValidationParameters.NameClaimType = "username";
    });


builder.Services.AddGrpc();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline


app.UseAuthentication(); // Add authentication middleware to the pipeline

app.UseAuthorization(); // Add authorization middleware to the pipeline

// Map controller routes
app.MapControllers();
app.MapGrpcService<GrpcAuctionService>();

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