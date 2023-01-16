using IdentityService.Responses;

namespace IdentityService.Services
{
    public interface IAccountService
    {
        System.Threading.Tasks.Task<ServiceRedirectResponse> ExternalLoginCallback(string returnUrl);
    }
}