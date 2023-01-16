using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace IdentityService;

public static class Config
{
    public const int LIFETIME_5MINS = 3600;

    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "Germany"
            };

            return new List<TestUser>
            {
                new()
                {
                    SubjectId = "818727",
                    Username = "alice",
                    Password = "alice",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.Role, "admin"),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                            IdentityServerConstants.ClaimValueTypes.Json)
                    }
                },
                new()
                {
                    SubjectId = "88421113",
                    Username = "bob",
                    Password = "bob",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.Role, "user"),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                            IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }
    }

    public static IEnumerable<IdentityResource> IdentityResources => new[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResource
        {
            Name = "role",
            UserClaims = new List<string> { "role" }
        }
    };

    public static IEnumerable<ApiScope> ApiScopes => new[]
    {
        new ApiScope("read:weatherforecast")
    };

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("introspection")
        {
            Scopes = new List<string>
            {
                "read:weatherforecast"
            },
            ApiSecrets = new List<Secret> { new("introspection_secret".Sha256()) },
        }
    };

    public static IEnumerable<Client> Clients => new[]
    {
        new Client
        {
            ClientId = "ids.ws",
            ClientName = "Machine2Machine/Ws",
            AllowAccessTokensViaBrowser = false,
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("ids.ws.client.secret".Sha256()) },
            AccessTokenLifetime = LIFETIME_5MINS,
            AllowedScopes =
            {
                "read:weatherforecast",
            },
        },

        new Client
        {
            ClientId = "ids.frontend",
            ClientName = "Frontend",
            AllowAccessTokensViaBrowser=true,
            AllowedGrantTypes= GrantTypes.Code,
            RequireClientSecret = false,
            RequirePkce = false,
            RedirectUris =
            {
                "https://localhost:4200"
            },
            RequireConsent = false,
            AccessTokenLifetime = LIFETIME_5MINS,
            AllowedScopes =
            {
                "read:weatherforecast",
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile
            },
            EnableLocalLogin = false,
            IdentityProviderRestrictions =
            {
                "Facebook"
            },
            AccessTokenType = AccessTokenType.Jwt
        },
    };
}