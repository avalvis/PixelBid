using BiddingService.Consumers;
using BiddingService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add MassTransit to the services and configure it
builder.Services.AddMassTransit(x =>
{
    // Add the consumers from the specified namespace
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    // Configure the endpoint name formatter to use kebab case
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));

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


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHostedService<CheckAuctionFinished>();

builder.Services.AddScoped<GrpcAuctionClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

await DB.InitAsync("BidDb", MongoClientSettings
        .FromConnectionString(builder.Configuration.GetConnectionString("BidDbConnection")));

app.Run();
