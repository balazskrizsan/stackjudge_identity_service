using Stackjudge_Identity_Server.Enums;

namespace Stackjudge_Identity_Server.ValueObjects;

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