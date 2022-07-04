using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace Stackjudge_Identity_Server;

public static class Config
{
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
        new ApiScope("sj"),
        new ApiScope("sj.aws"),
        new ApiScope("sj.aws.ec2"),
        new ApiScope("sj.aws.ec2.upload_company_logo"),
        new ApiScope("sj.aws.ec2.upload_company_map"),
        new ApiScope("sj.aws.ses"),
        new ApiScope("sj.aws.ses.send_mail"),
    };

    // public static IEnumerable<ApiResource> ApiResources => new[]
    // {
    //     new ApiResource("weatherapi")
    //     {
    //         Scopes = new List<string> { "sj.aws.ec2.upload_company_logo" },
    //         ApiSecrets = new List<Secret> { new("ScopeSecret".Sha256()) },
    //         UserClaims = new List<string> { "role" }
    //     }
    // };

    public static IEnumerable<Client> Clients => new[]
    {
        // m2m client credentials flow client
        new Client
        {
            ClientId = "sj.aws",
            ClientName = "Machine2Machine/Sj/Aws",
            AllowAccessTokensViaBrowser = false,
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("m2m.client.secret".Sha256()) },
            AllowedScopes =
            {
                "sj",
                "sj.aws",
                "sj.aws.ec2",
                "sj.aws.ec2.upload_company_logo",
                "sj.aws.ec2.upload_company_map",
                "sj.aws.ses",
                "sj.aws.ses.send_mail"
            },
        },
    };
}