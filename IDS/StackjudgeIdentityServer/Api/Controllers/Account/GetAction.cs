using System.Linq;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackjudgeIdentityServer.Api.Builders;
using StackjudgeIdentityServer.Api.Response;
using StackjudgeIdentityServer.Api.ValueObjects;
using StackjudgeIdentityServer.Data;
using StackjudgeIdentityServer.Data.Entity;

namespace StackjudgeIdentityServer.Api.Controllers.Account;

[ApiController]
public class GetAction
{
    private readonly AppDbContext appDbContext;

    public GetAction(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    [HttpPost]
    [Route("/api/account/{userId}")]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public ResponseEntity<ResponseData<GetResponse>> Get(string userId)
    {
        var responseEntityBuilder = new ResponseEntityBuilder<GetResponse>
        {
            Data = new GetResponse
            {
                IdsUser = appDbContext.ExtendedUsers
                    .Where(user => user.UserId == userId)
                    .Join(appDbContext.Users,
                        eu => eu.UserId,
                        anu => anu.Id,
                        (eu, anu) => new IdsUser
                        (
                            eu.UserId,
                            anu.UserName,
                            anu.NormalizedUserName,
                            anu.Email,
                            anu.EmailConfirmed,
                            eu.ProfileUrl
                        ))
                    .First()
            }
        };

        return responseEntityBuilder.Build();
    }
}