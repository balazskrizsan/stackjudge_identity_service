using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stackjudge_Identity_Server.ViewModels;
using System.Threading.Tasks;

namespace Stackjudge_Identity_Server.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [BindProperty]
        public LoginViewModel? LoginData { get; set; }

        public async Task<PageResult> OnGet(string returnUrl)
        {
            var externalProviders = await signInManager.GetExternalAuthenticationSchemesAsync();

            LoginData = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalProviders = externalProviders
            };

            return Page();
        }
    }
}
