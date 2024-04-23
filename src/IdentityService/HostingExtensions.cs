using Duende.IdentityServer;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

internal static class HostingExtensions
{
    // This method configures the services for the application
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add support for Razor Pages
        builder.Services.AddRazorPages();

        // Add the application's DbContext and configure it to use PostgreSQL
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add Identity services and configure them to use the application's DbContext
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Add IdentityServer services and configure them
        builder.Services
            .AddIdentityServer(options =>
            {
                // Configure IdentityServer to raise different types of events
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                if (builder.Environment.IsEnvironment("Docker"))
                {
                    options.IssuerUri = "http//identity-svc";
                }


                // Uncomment the following line to emit a static audience claim
                // options.EmitStaticAudienceClaim = true;
            })
            // Add in-memory identity resources, API scopes, and clients
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients(builder.Configuration))
            // Add ASP.NET Identity support
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>();

        // Configure the application's cookie settings
        builder.Services.ConfigureApplicationCookie(options =>
        {   // Configure the cookie to be essential for authentication
            options.Cookie.SameSite = SameSiteMode.Lax;
        });

        // Add authentication services
        builder.Services.AddAuthentication();

        // Build the application
        return builder.Build();
    }

    // This method configures the HTTP request pipeline for the application
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Use Serilog for request logging
        app.UseSerilogRequestLogging();

        // Use the developer exception page in development environments
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Use static files, routing, IdentityServer, and authorization middleware
        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        // Map Razor Pages and require authorization for them
        app.MapRazorPages()
            .RequireAuthorization();

        // Return the configured application
        return app;
    }
}