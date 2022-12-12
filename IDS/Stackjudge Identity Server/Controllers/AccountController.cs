using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stackjudge_Identity_Server.Responses;
using Stackjudge_Identity_Server.Services;
using Stackjudge_Identity_Server.ViewModels;
using System;
using System.Threading.Tasks;

namespace Stackjudge_Identity_Server.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IAccountService accountService;

        public AccountController(SignInManager<IdentityUser> signInManager, IAccountService accountService)
        {
            this.signInManager = signInManager;
            this.accountService = accountService;
        }

        public async Task<IActionResult> Login(string returnUrl)
        {
            var externalProviders = await signInManager.GetExternalAuthenticationSchemesAsync();

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ExternalProviders = externalProviders;

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalProviders = externalProviders
            });
        }

        public  IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUri = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var response = await accountService.ExternalLoginCallback(returnUrl);

            switch (response.Type)
            {
                case ServiceRedirectResponseEnumTypes.Redirect:
                    return Redirect(response.Url);
                case ServiceRedirectResponseEnumTypes.RedirectToAction:
                    return RedirectToAction(response.Url);
                default:
                    throw new Exception($"Missing redirect type: {response.Type}");
            }
        }
    }
}
