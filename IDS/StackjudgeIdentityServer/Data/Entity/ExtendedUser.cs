using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StackjudgeIdentityServer.Data.Entity;

[Table("ExtendedUsers")]
[Index(nameof(ExternalId), IsUnique = true)]
public class ExtendedUser
{
    [Key, Column] public string UserId { get; set; }
    [Column] public string ExternalId { get; set; }
    [Column] public string AccessToken { get; set; }
    [Column] public string ProfileUrl { get; set; }

    public ExtendedUser(string userId, string externalId, string accessToken, string profileUrl)
    {
        UserId = userId;
        ExternalId = externalId;
        AccessToken = accessToken;
        ProfileUrl = profileUrl;
    }
}