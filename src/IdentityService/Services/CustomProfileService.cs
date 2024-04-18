using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService
{
    // Custom implementation of IProfileService that adds custom claims to the profile
    public class CustomProfileService : IProfileService
    {
        // UserManager to interact with user data
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor that takes UserManager as a dependency 
        public CustomProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // Asynchronously gets profile data for the context
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // Get the user associated with the context
            var user = await _userManager.GetUserAsync(context.Subject);

            // Ensure the user is not null before proceeding
            if (user == null)
            {
                return;
            }

            // Get the existing claims for the user
            var existingClaims = await _userManager.GetClaimsAsync(user);

            // Create a new list of claims
            var claims = new List<Claim>();

            // Check if the username is not null before adding it as a claim
            if (!string.IsNullOrEmpty(user.UserName))
            {
                claims.Add(new Claim("username", user.UserName));
            }

            // Add the new claims to the context
            context.IssuedClaims.AddRange(claims);

            // Add the existing name claim to the context, check if it exists first
            var nameClaim = existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name);
            if (nameClaim != null)
            {
                context.IssuedClaims.Add(nameClaim);
            }
        }

        // Checks if the user is active
        public Task IsActiveAsync(IsActiveContext context)
        {
            // For this implementation, we're not checking if the user is active, so we just return a completed task
            return Task.CompletedTask;
        }
    }
}
