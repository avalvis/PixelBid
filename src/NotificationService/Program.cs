using MassTransit;
using NotificationService;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add MassTransit to the services and configure it
builder.Services.AddMassTransit(x =>
{
    // Add the consumers from the assembly containing the AuctionCreatedConsumer
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    // Configure the endpoint name formatter to use kebab case
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("nt", false));

    // Configure MassTransit to use RabbitMQ as the message broker
    x.UsingRabbitMq((context, cfg) =>
    {


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

builder.Services.AddSignalR();


var app = builder.Build();

app.MapHub<NotificationHub>("/notifications");



app.Run();
