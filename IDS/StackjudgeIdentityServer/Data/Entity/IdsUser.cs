namespace StackjudgeIdentityServer.Data.Entity;

public class IdsUser
{
    public string id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }

    public IdsUser(
        string id,
        string userName,
        string normalizedUserName,
        string email,
        bool emailConfirmed,
        string profileUrl
    )
    {
        this.id = id;
        UserName = userName;
        NormalizedUserName = normalizedUserName;
        Email = email;
        EmailConfirmed = emailConfirmed;
        ProfileUrl = profileUrl;
    }

    public string ProfileUrl { get; set; }
}