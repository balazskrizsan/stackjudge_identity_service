using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace StackjudgeIdentityServer;

public static class OidcConfig
{
    public const int LIFETIME_10SEC = 10;
    public const int LIFETIME_1MIN = 60;
    public const int LIFETIME_5MINS = 300;
    public const int LIFETIME_1HOUR = 3600;
    public const int LIFETIME_2HOURS = 7200;
    public const int LIFETIME_4HOURS = 14400;

    private static Dictionary<string, string> exchangeMap = new()
    {
        {"xc/sj.aws.s3", "sj.aws.s3"}
    };

    public static bool IsValidateExchange(string exchangeFrom, string exchangeTo)
    {
        exchangeMap.TryGetValue(exchangeFrom, out string exchangeFromValue);
        if (null == exchangeFromValue)
        {
            return false;
        }

        return exchangeFromValue == exchangeTo;
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
        new ApiScope("sj.ids"),
        new ApiScope("sj.ids.api"),
        new ApiScope("sj.frontend"),
        new ApiScope("sj.aws"),
        new ApiScope("sj.aws.s3"),
        new ApiScope("xc/sj.aws.s3"),
        new ApiScope("xc/sj.aws.ses"),
        new ApiScope("sj.aws.ec2"),
        new ApiScope("sj.aws.ec2.upload_company_logo"),
        new ApiScope("sj.aws.ec2.upload_company_map"),
        new ApiScope("sj.aws.ses"),
        new ApiScope("sj.aws.ses.send_mail"),
        new ApiScope("sj.notification"),
        new ApiScope("sj.notification.send_push"),
        new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
    };

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("sj.resource.aws")
        {
            Scopes = new List<string>
            {
                "sj",
                "sj.aws",
                "xc/sj.aws.s3",
                "sj.aws.s3",
            },
            ApiSecrets = new List<Secret> { new("sj.resource.aws".Sha256()) },
        },
        new ApiResource("sj.resource.frontend")
        {
            Scopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "sj",
                "sj.frontend",
            },
            ApiSecrets = new List<Secret> { new("sj_aws_scopes".Sha256()) },
        },
        new ApiResource("sj.resource.ids")
        {
            Scopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.LocalApi.ScopeName,
                "sj",
                "sj.ids",
                "sj.ids.api",
            },
            ApiSecrets = new List<Secret> { new("sj_aws_scopes".Sha256()) },
        }
    };

    public static IEnumerable<Client> Clients => new[]
    {
        new Client
        {
            ClientId = "sj.aws",
            ClientName = "Machine2Machine/Sj/Aws",
            AllowAccessTokensViaBrowser = false,
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("sj.aws.client.secret".Sha256()) },
            AccessTokenLifetime = LIFETIME_4HOURS,
            AllowedScopes =
            {
                "sj",
                "sj.aws",
                "xc/sj.aws.s3",
                "xc/sj.aws.ses",
                "sj.aws.ec2",
                "sj.aws.ec2.upload_company_logo",
                "sj.aws.ec2.upload_company_map",
                "sj.aws.ses",
                "sj.aws.ses.send_mail",
            },
        },
        new Client
        {
            ClientId = "sj.notification",
            ClientName = "Machine2Machine/Sj/Aws",
            AllowAccessTokensViaBrowser = false,
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("sj.notification.client.secret".Sha256()) },
            AccessTokenLifetime = LIFETIME_4HOURS,
            AllowedScopes =
            {
                "sj",
                "sj.notification",
                "sj.notification.send_push",
            },
        },
        new Client
        {
            ClientId = "sj.frontend",
            ClientName = "Code/Sj/Frontend",
            AllowAccessTokensViaBrowser = true,
            AllowedGrantTypes = GrantTypes.Code,
            RequireClientSecret = false,
            RequirePkce = false, // go true
            RedirectUris = { "https://localhost:4200" },
            PostLogoutRedirectUris = { },
            AllowedCorsOrigins = { "https://localhost:4200" },
            RequireConsent = false,
            AccessTokenLifetime = LIFETIME_4HOURS,
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "sj",
                "sj.frontend",
            },
            EnableLocalLogin = false,
            IdentityProviderRestrictions = { "Facebook" },
            AccessTokenType = AccessTokenType.Jwt
        },
        new Client
        {
            ClientId = "sj.ids.api",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("sj.ids.api.secret".Sha256()) },
            AllowOfflineAccess = true,
            RequireClientSecret = false,
            RedirectUris = { },
            PostLogoutRedirectUris = { },
            RequirePkce = true,
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.LocalApi.ScopeName,
                "sj",
                "sj.ids",
                "sj.ids.api",
            }
        },
        new Client
        {
            ClientId = "sj.exchange",
            AllowedGrantTypes =  new[] { "token_exchange" },
            ClientSecrets = { new Secret("sj.exchange".Sha256()) },
            AllowOfflineAccess = true,
            RequireClientSecret = false,
            RedirectUris = { },
            PostLogoutRedirectUris = { },
            RequirePkce = true,
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.LocalApi.ScopeName,
                "sj",
                "sj.aws",
                "sj.aws.s3",
            }
        }
    };
}