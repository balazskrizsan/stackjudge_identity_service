using System.Threading.Tasks;
using StackjudgeIdentityServer.ValueObjects;

namespace StackjudgeIdentityServer.Services;

public interface IAccountService
{
    Task<ServiceRedirectResponse> ExternalLoginCallback(string returnUrl);
}