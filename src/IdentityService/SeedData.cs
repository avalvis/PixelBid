using System.Security.Claims;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

public class SeedData
{
    // This method ensures that the database is seeded with initial data
    public static void EnsureSeedData(WebApplication app)
    {
        // Create a new scope for retrieving services
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        // Get the application database context
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply any pending migrations for the context to the database
        context.Database.Migrate();

        // Get the user manager service
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // If there are any users in the database, exit the method
        if (userMgr.Users.Any()) return;

        // Try to find a user named "maria"
        var maria = userMgr.FindByNameAsync("maria").Result;

        // If "maria" doesn't exist, create her
        if (maria == null)
        {
            maria = new ApplicationUser
            {
                UserName = "maria",
                Email = "MariaPapadopoulou@email.com",
                EmailConfirmed = true,
            };

            // Create "maria" in the database
            var result = userMgr.CreateAsync(maria, "Pass123$").Result;

            // If the creation failed, throw an exception
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            // Add a claim to "maria"
            result = userMgr.AddClaimsAsync(maria, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "maria Papadopoulou"),
                        }).Result;

            // If adding the claim failed, throw an exception
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            // Log that "maria" was created
            Log.Debug("maria created");
        }
        else
        {
            // Log that "maria" already exists
            Log.Debug("maria already exists");
        }

        // Try to find a user named "tony"
        var tony = userMgr.FindByNameAsync("tony").Result;

        // If "tony" doesn't exist, create him
        if (tony == null)
        {
            tony = new ApplicationUser
            {
                UserName = "tony",
                Email = "TonyValvis@email.com",
                EmailConfirmed = true
            };

            // Create "tony" in the database
            var result = userMgr.CreateAsync(tony, "Pass123$").Result;

            // If the creation failed, throw an exception
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            // Add a claim to "tony"
            result = userMgr.AddClaimsAsync(tony, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Tony Valvis"),
                        }).Result;

            // If adding the claim failed, throw an exception
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            // Log that "tony" was created
            Log.Debug("tony created");
        }
        else
        {
            // Log that "tony" already exists
            Log.Debug("tony already exists");
        }
    }
}