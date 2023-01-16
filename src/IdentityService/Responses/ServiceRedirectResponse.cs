namespace IdentityService.Responses;

public class ServiceRedirectResponse
{
    public ServiceRedirectResponseEnumTypes Type { get; }
    public string Url { get; }

    public ServiceRedirectResponse(ServiceRedirectResponseEnumTypes type, string url)
    {
        Type = type;
        Url = url;
    }
}
