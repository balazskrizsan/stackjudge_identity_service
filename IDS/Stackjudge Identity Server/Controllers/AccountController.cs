using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Stackjudge_Identity_Server.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IIdentityServerInteractionService interactionService;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IIdentityServerInteractionService interactionService
    )
    {
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
        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction("Login");
        }

        var result = await signInManager
            .ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

        if (result.Succeeded)
        {
            return Redirect(returnUrl);
        }

        var username = info.Principal.FindFirst(ClaimTypes.Name.Replace(" ", "_")).Value;
        return View("ExternalRegister", new ExternalRegisterViewModel
        {
            Username = username,
            ReturnUrl = returnUrl
        });
    }

    public async Task<IActionResult> ExternalRegister(ExternalRegisterViewModel vm)
    {
        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction("Login");
        }

        var user = new IdentityUser(vm.Username);
        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            return View(vm);
        }

        result = await userManager.AddLoginAsync(user, info);

        if (!result.Succeeded)
        {
            return View(vm);
        }

        await signInManager.SignInAsync(user, false);

        return Redirect(vm.ReturnUrl);
    }
}