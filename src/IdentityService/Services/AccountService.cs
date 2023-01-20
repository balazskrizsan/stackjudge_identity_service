using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using IdentityService.Responses;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IdentityService.Services;

public class AccountService : IAccountService
{
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IIdentityServerInteractionService interactionService;
    private readonly AppDbContext appDbContext;

    public AccountService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IIdentityServerInteractionService interactionService,
        AppDbContext appDbContext
    )
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.interactionService = interactionService;
        this.appDbContext = appDbContext;
    }

    public async Task<ServiceRedirectResponse> ExternalLoginCallback(string returnUrl)
    {
        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return new ServiceRedirectResponse(
                ServiceRedirectResponseEnumTypes.RedirectToAction,
                "Login"
                );
            // return RedirectToAction("Login");
        }

        var result = await signInManager
            .ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

        if (result.Succeeded)
        {
            return new ServiceRedirectResponse(
                ServiceRedirectResponseEnumTypes.Redirect,
                returnUrl
            );
            // return Redirect(returnUrl);
        }

        var principal = info.Principal;
        var picture = principal.FindFirst(JwtClaimTypes.Picture)?.Value;
        var externalId = principal.FindFirst(JwtClaimTypes.Id)?.Value;
        var accessToken = principal.FindFirst(JwtClaimTypes.AccessTokenHash)?.Value;
        var username = principal.FindFirst(ClaimTypes.Name)?.Value;
        username = Regex.Replace(username, "[^a-zA-Z]", "");

        var user = new IdentityUser(username);
        var createResult = await userManager.CreateAsync(user);

        // @todo: add back
        // if (!result.Succeeded) return View(vm);

        createResult = await userManager.AddLoginAsync(user, info);

        var currentExtendedUser = appDbContext.ExtendedUsers.FirstOrDefault(eu => eu.UserId == externalId);

        if (currentExtendedUser == null)
        {
            appDbContext.ExtendedUsers.Add(new ExtendedUser(user.Id, externalId, accessToken, picture));
        }
        else
        {
            currentExtendedUser.UpdateProfile(picture, accessToken);
        }

        await appDbContext.SaveChangesAsync();

        // @todo: add back
        // if (!createResult.Succeeded) return View(vm);

        await signInManager.SignInAsync(user, false);

        return new ServiceRedirectResponse(
            ServiceRedirectResponseEnumTypes.Redirect,
            returnUrl
        );
        // return Redirect(returnUrl);
    }
}
