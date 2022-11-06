using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace StackjudgeIdentityServer.Controllers;

public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
    public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }
}