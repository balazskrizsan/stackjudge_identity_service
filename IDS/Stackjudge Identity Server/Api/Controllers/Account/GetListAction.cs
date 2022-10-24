using System.Linq;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stackjudge_Identity_Server.Api.Builders;
using Stackjudge_Identity_Server.Api.Response;
using Stackjudge_Identity_Server.Api.ValueObjects;
using Stackjudge_Identity_Server.Data;
using Stackjudge_Identity_Server.Data.Entity;

namespace Stackjudge_Identity_Server.Api.Controllers.Account;

[ApiController]
public class GetListAction
{
    private readonly AppDbContext appDbContext;

    public GetListAction(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    [HttpPost]
    [Route("/api/account/list")]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public ResponseEntity<ResponseData<GetResponse>> GetInfoList()
    {
        var responseEntityBuilder = new ResponseEntityBuilder<GetResponse>
        {
            Data = new GetResponse
            {
                ExtendedUsers = appDbContext.ExtendedUsers
                    .Join(
                        appDbContext.Users,
                        eu => eu.UserId,
                        anu => anu.Id,
                        (eu, anu) => new
                        {
                            id = eu.UserId,
                            userName = anu.UserName,
                            normalizedUserName = anu.NormalizedUserName,
                            email = anu.Email,
                            emailConfirmed = anu.EmailConfirmed,
                            profileUrl = eu.ProfileUrl
                        }
                    )
                    .Select(j => new IdsUser(
                        j.id,
                        j.userName,
                        j.normalizedUserName,
                        j.email,
                        j.emailConfirmed,
                        j.profileUrl
                    ))
                    .ToList()
            }
        };
        return responseEntityBuilder.Build();
    }
}