using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.Extensions.Logging;

namespace StackjudgeIdentityServer.Services;

public class TokenExchangeGrantValidatorService : IExtensionGrantValidator
{
    private readonly ITokenValidator _validator;
    private readonly ILogger<TokenExchangeGrantValidatorService> _logger;

    public TokenExchangeGrantValidatorService(
        ITokenValidator validator,
        ILogger<TokenExchangeGrantValidatorService> logger
    )
    {
        _validator = validator;
        _logger = logger;
    }

    public string GrantType => "token_exchange";

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        _logger.LogInformation("========  Validation started");

        var userToken = context.Request.Raw.Get("token");
        var exchangeFrom = context.Request.Raw.Get("exchange_from");
        var exchangeTo = context.Request.Raw.Get("scope");
        var authMethod = GrantType + " method";

        _logger.LogInformation("User Token: " + userToken);
        _logger.LogInformation("Exchange From: " + exchangeFrom);
        _logger.LogInformation("Exchange to: " + exchangeTo);

        if (!ScopeService.IsValidExchange(exchangeFrom, exchangeTo))
        {
            _logger.LogError("IsValidExchange error");
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidScope);
        
            return;
        }

        if (string.IsNullOrEmpty(userToken))
        {
            _logger.LogError("IsNullOrEmpty error");
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            return;
        }

        var result = await _validator.ValidateAccessTokenAsync(userToken);
        if (result.IsError)
        {
            _logger.LogError("ValidateAccessTokenAsync error");
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            return;
        }

        Dictionary<string, object> customResponse = new Dictionary<string, object>();
        customResponse.Add("exchange_from", exchangeFrom);
        customResponse.Add("exchange_to", exchangeTo);

        context.Result = new GrantValidationResult(exchangeFrom, authMethod, new List<Claim>(), "idp", customResponse);
    }
}