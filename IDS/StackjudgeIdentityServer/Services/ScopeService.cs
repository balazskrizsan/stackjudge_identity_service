using System.Collections.Generic;
using System.Linq;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using StackjudgeIdentityServer.ValueObjects;

namespace StackjudgeIdentityServer.Services;

public static class ScopeService
{
    // @todo: add unique check
    private static List<Scope> scopes = new()
    {
        new(
            IdentityServerConstants.StandardScopes.OpenId,
            new List<string> { "sj.frontend" },
            new List<string>(),
            new List<string>()),
        new(
            IdentityServerConstants.StandardScopes.Profile,
            new List<string> { "sj.frontend" },
            new List<string> { },
            new List<string>()),
        new(
            IdentityServerConstants.LocalApi.ScopeName,
            new List<string> { },
            new List<string> { },
            new List<string>()),
        new(
            IdentityServerConstants.StandardScopes.OfflineAccess,
            new List<string> { "sj.frontend" },
            new List<string> { },
            new List<string>()),
        new(
            "sj",
            new List<string> { "sj.frontend" },
            new List<string> { "sj.frontend" },
            new List<string>()),
        new(
            "IdentityServerApi",
            new List<string> { "sj.ids.api" },
            new List<string> { "sj.ids.api" },
            new List<string>()),
        new(
            "IdentityServerAccessToken",
            new List<string> { "sj.ids.api" },
            new List<string> { "sj.ids.api" },
            new List<string>()),
        // Frontend
        new(
            "sj.frontend",
            new List<string> { "sj.frontend" },
            new List<string> { "sj.frontend" },
            new List<string>()),
        // AWS
        new(
            "xc/sj.aws",
            new List<string> { "sj.aws" },
            new List<string> { "sj.aws" },
            new List<string> { "sj.aws" }
        )
    };

    public static bool IsValidExchange(string exchangeFrom, string exchangeTo)
    {
        Dictionary<string, List<string>> exchangeMap = scopes
            .Where(s => s.ExchangeableToList.Any())
            .ToDictionary(s => s.Name, s => s.ExchangeableToList);

        exchangeMap.TryGetValue(exchangeFrom, out List<string> exchangeableTo);
        if (null == exchangeableTo || !exchangeableTo.Any())
        {
            return false;
        }

        return exchangeableTo.Contains(exchangeTo);
    }

    public static List<string> GetExchangeScopes()
    {
        return scopes
            .Where(x => x.ExchangeableToList.Any())
            .SelectMany(x => x.ExchangeableToList)
            .ToList();
    }

    public static List<string> GetScopeNamesWithExchangeableTo()
    {
        return scopes
            .Where(x => x.ExchangeableToList.Any())
            .Select(x => x.Name)
            .ToList();
    }

    public static List<string> GetScopesByApiResourceName(string apiResourceName)
    {
        return scopes
            .Where(x => x.ApiResourceNames.Contains(apiResourceName))
            .Select(x => x.Name)
            .ToList();
    }

    public static List<ApiScope> GetApiScopesHasApiResource()
    {
        var names = scopes
            .Where(x => x.ApiResourceNames.Any())
            .Select(x => new ApiScope(x.Name))
            .ToList();

        var exchangeableToLists = scopes
            .Where(x => x.ApiResourceNames.Any())
            .SelectMany(x => x.ExchangeableToList.Select(x => new ApiScope(x)))
            .ToList();

        names.AddRange(exchangeableToLists);

        return names;
    }

    public static List<string> GetScopesByClientId(string clientId)
    {
        var names = scopes
            .Where(x => x.ClientIds.Contains(clientId))
            .Select(x => x.Name)
            .ToList();

        var exchangeableToLists = scopes
            .Where(x => x.ClientIds.Contains(clientId))
            .SelectMany(x => x.ExchangeableToList)
            .ToList();

        names.AddRange(exchangeableToLists);

        return names;
    }
}