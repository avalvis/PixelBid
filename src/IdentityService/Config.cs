using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("auctionApp", "Auction API full access"),
        };

    public static IEnumerable<Client> Clients(IConfiguration config) =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "postman",
                AllowedScopes = {"openid", "profile", "auctionApp"},
                // a random redirect uri since it is not needed for postman
                RedirectUris = {"https://www.example.com/callback"},
                ClientSecrets = new[] {new Secret("postman".Sha256())},
                AllowedGrantTypes = {GrantType.ResourceOwnerPassword}

            }
        };
}
