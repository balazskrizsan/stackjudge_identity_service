using System;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackjudgeIdentityServer.Enums;
using StackjudgeIdentityServer.Services;

namespace StackjudgeIdentityServer.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService accountService;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IIdentityServerInteractionService interactionService;

    public AccountController(
        IAccountService accountService,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IIdentityServerInteractionService interactionService
    )
    {
        this.accountService = accountService;
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.interactionService = interactionService;
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        await signInManager.SignOutAsync();

        var logoutRequest = await interactionService.GetLogoutContextAsync(logoutId);

        if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
        {
            return RedirectToAction("Index", "Home");
        }

        return Redirect(logoutRequest.PostLogoutRedirectUri);
    }

    [HttpGet]
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

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        // check if the model is valid

        var result = await signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

        if (result.Succeeded)
        {
            return Redirect(vm.ReturnUrl);
        }
        else if (result.IsLockedOut)
        {
        }

        return View();
    }

    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
        return View(new RegisterViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var user = new IdentityUser(vm.Username);
        var result = await userManager.CreateAsync(user, vm.Password);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, false);

            return Redirect(vm.ReturnUrl);
        }

        return View();
    }

    public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
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