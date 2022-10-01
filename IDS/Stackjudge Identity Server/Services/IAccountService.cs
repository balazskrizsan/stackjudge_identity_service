using System.Threading.Tasks;
using Stackjudge_Identity_Server.ValueObjects;

namespace Stackjudge_Identity_Server.Services;

public interface IAccountService
{
    Task<ServiceRedirectResponse> ExternalLoginCallback(string returnUrl);
}