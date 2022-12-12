using Stackjudge_Identity_Server.Responses;

namespace Stackjudge_Identity_Server.Services
{
    public interface IAccountService
    {
        System.Threading.Tasks.Task<ServiceRedirectResponse> ExternalLoginCallback(string returnUrl);
    }
}