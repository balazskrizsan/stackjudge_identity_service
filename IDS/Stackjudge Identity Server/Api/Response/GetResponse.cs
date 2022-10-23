using System.Collections.Generic;
using Stackjudge_Identity_Server.Data.Entity;

namespace Stackjudge_Identity_Server.Api.Response;

public class GetResponse
{
    public List<IdsUser> ExtendedUsers { get; set; }
}