// Config.cs
using Duende.IdentityServer.Models;

// Define the namespace for the configuration
namespace IdentityService
{
    public static class Config
    {
        // Define the IdentityResources property which returns a collection of identity resources
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                // Include the OpenId and Profile identity resources
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        // Define the ApiScopes property which returns a collection of API scopes
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                // Include the "auctionApp" API scope with full access
                new ApiScope("auctionApp", "Auction API full access"),
            };

        // Define the Clients method which takes a configuration and returns a collection of clients
        public static IEnumerable<Client> Clients(IConfiguration config) =>
            new Client[]
            {
                // Define the "postman" client for testing APIs with postman
                new Client
                {
                    ClientId = "postman",
                    ClientName = "postman",
                    AllowedScopes = {"openid", "profile", "auctionApp"},
                    // Include a random redirect URI since it is not needed for postman
                    RedirectUris = {"https://www.example.com/callback"},
                    ClientSecrets = new[] {new Secret("postman".Sha256())},
                    AllowedGrantTypes = {GrantType.ResourceOwnerPassword}
                },
                // Define the "nextclient" client for the Next.js frontend application
                new Client
                {
                    ClientId = "nextclient",
                    ClientName = "nextclient",
                    ClientSecrets = {new Secret("nextclient".Sha256())},
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = false,
                    RedirectUris = {"http://localhost:3000/api/auth/callback/id-server"},
                    AllowOfflineAccess = true,
                    AllowedScopes = {"openid", "profile", "auctionApp"},
                    AccessTokenLifetime = 3600*24*30,

                }
            };
    }
}
