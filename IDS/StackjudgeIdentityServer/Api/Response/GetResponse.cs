using System.Collections.Generic;
using StackjudgeIdentityServer.Data.Entity;

namespace StackjudgeIdentityServer.Api.Response;

public class GetResponse
{
    public List<IdsUser> ExtendedUsers { get; set; }
}