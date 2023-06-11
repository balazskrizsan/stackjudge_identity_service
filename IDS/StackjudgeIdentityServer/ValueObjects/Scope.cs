using System.Collections.Generic;

namespace StackjudgeIdentityServer.ValueObjects;

public record Scope(
    string Name,
    List<string> ClientIds,
    List<string> ApiResourceNames,
    List<string> ExchangeableToList = null
)
{
    public List<string> ExchangeableToList { get; init; } = ExchangeableToList ?? new List<string>();
}