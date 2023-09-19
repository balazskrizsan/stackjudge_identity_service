using System.Collections.Generic;
using System.Linq;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using StackjudgeIdentityServer.Services;

namespace StackjudgeIdentityServer;

public static class OidcConfig
{
    private const int LIFETIME_10SEC = 10;
    private const int LIFETIME_1MIN = 60;
    private const int LIFETIME_5MINS = 300;
    private const int LIFETIME_1HOUR = 3600;
    private const int LIFETIME_2HOURS = 7200;
    private const int LIFETIME_4HOURS = 14400;

    private const string CLIENT_EXCHANGE = "sj.exchange";
    private const string CLIENT_AWS = "sj.aws";
    private const string API_RESOURCE_AWS = "sj.resource.aws";
    private const string CLIENT_FRONTEND = "sj.frontend";
    private const string API_RESOURCE_FRONTEND = "sj.resource.frontend";
    private const string CLIENT_NOTIFICATION = "sj.notification";
    private const string CLIENT_SJ_IDS_API = "sj.ids.api";

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

    // public static IEnumerable<ApiScope> ApiScopes => ScopeService.GetAllScopesHasApiResource();
    public static IEnumerable<ApiScope> ApiScopes => ScopeService.GetApiScopesHasApiResource();

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource(API_RESOURCE_AWS)
        {
            Scopes = ScopeService.GetScopesByClientId(CLIENT_AWS),
            ApiSecrets = new List<Secret> { new(API_RESOURCE_AWS.Sha256()) },
        },
        new ApiResource(API_RESOURCE_FRONTEND)
        {
            Scopes = ScopeService.GetScopesByClientId(CLIENT_FRONTEND),
            ApiSecrets = new List<Secret> { new(API_RESOURCE_FRONTEND.Sha256()) },
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
            ApiSecrets = new List<Secret> { new("sj.resource.ids".Sha256()) },
        }
    };

    private static List<Client> Clients => new()
    {
        new()
        {
            ClientId = CLIENT_AWS,
            ClientName = CLIENT_AWS,
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret(CLIENT_AWS.Sha256()) },
            AllowedScopes = ScopeService.GetScopesByClientId(CLIENT_AWS),
            AccessTokenLifetime = LIFETIME_4HOURS,
            AllowAccessTokensViaBrowser = false,
        },
        new()
        {
            ClientId = CLIENT_NOTIFICATION,
            ClientName = CLIENT_NOTIFICATION,
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret(CLIENT_NOTIFICATION.Sha256()) },
            AllowedScopes =
            {
                "sj",
                "sj.notification",
                "sj.notification.send_push",
            },
            AllowAccessTokensViaBrowser = false,
            AccessTokenLifetime = LIFETIME_4HOURS,
        },
        new()
        {
            ClientId = CLIENT_FRONTEND,
            ClientName = CLIENT_FRONTEND,
            AllowedGrantTypes = GrantTypes.Code,
            RequireClientSecret = false,
            AllowedScopes = ScopeService.GetScopesByClientId(CLIENT_FRONTEND),
            AllowAccessTokensViaBrowser = true,
            RequirePkce = false, // go true
            RedirectUris = { "https://localhost:4200" },
            PostLogoutRedirectUris = { },
            AllowedCorsOrigins = { "https://localhost:4200" },
            RequireConsent = false,
            AccessTokenLifetime = LIFETIME_4HOURS,
            EnableLocalLogin = false,
            IdentityProviderRestrictions = { "Facebook" },
            AccessTokenType = AccessTokenType.Jwt,
            RefreshTokenUsage = TokenUsage.ReUse,
            AllowOfflineAccess = true
        },
        new()
        {
            ClientId = CLIENT_SJ_IDS_API,
            ClientName = CLIENT_SJ_IDS_API,
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret(CLIENT_SJ_IDS_API.Sha256()) },
            AllowOfflineAccess = true,
            RequireClientSecret = false,
            RequirePkce = true,
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.LocalApi.ScopeName,
                IdentityServerConstants.LocalApi.AuthenticationScheme,
                "sj",
                "sj.ids",
                "sj.ids.api",
            }
        },
        new()
        {
            ClientId = CLIENT_EXCHANGE,
            ClientName = CLIENT_EXCHANGE,
            AllowedGrantTypes = new[] { "token_exchange" },
            ClientSecrets = { new Secret("sj.exchange".Sha256()) },
            AllowedScopes = ScopeService.GetExchangeScopes(),
            AllowOfflineAccess = true,
            RequireClientSecret = false,
            RequirePkce = true,
        }
    };

    public static IEnumerable<Client> GetClients()
    {
        return Clients;
    }

    public static List<string> GetClientIds()
    {
        return Clients.Select(c => c.ClientId).ToList();
    }
}