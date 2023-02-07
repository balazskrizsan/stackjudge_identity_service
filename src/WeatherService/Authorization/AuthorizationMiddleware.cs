using IdentityModel.Client;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text;

namespace WeatherService.Authentication
{
    public static class AuthorizationMiddleware
    {
        const string AuthScheme = "Bearer ";

        public static void UseIdentityServerAuthorization(this WebApplication app, string identityServiceCredentials) => app.Use(async (context, next) =>
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authorizationValue))
            {
                await Unauthorized(context);
                return;
            }

            if (!TryParseAccessToken(authorizationValue, out string? accessToken))
            {
                await Unauthorized(context);
                return;
            }

            var identityServiceClient = app.Services.GetService<IHttpClientFactory>()!.CreateClient("IdentityService");
            var discoveryReponse = await identityServiceClient.GetDiscoveryDocumentAsync();

            var introspectionRequest = new TokenIntrospectionRequest
            {
                Token = accessToken,
                Address = discoveryReponse.IntrospectionEndpoint
            };

            introspectionRequest.Headers.Add(
                "Authorization",
                $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(identityServiceCredentials))}");

            var introspectionResponse = await identityServiceClient.IntrospectTokenAsync(introspectionRequest);

            if (!introspectionResponse.IsActive)
            {
                await Unauthorized(context);
                return;
            }

            await next(context);
        });

        private static bool TryParseAccessToken(StringValues headerValue, out string? accessToken)
        {
            var rawAuthValue = headerValue.SingleOrDefault();
            accessToken = null;

            if (rawAuthValue is null || !rawAuthValue.StartsWith(AuthScheme))
            {
                return false;
            }

            var authParts = rawAuthValue.Split(AuthScheme);

            if (authParts.Length != 2)
            {
                return false;
            }

            accessToken = authParts[1];
            return true;
        }

        private static async Task Unauthorized(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.Headers["Content-Type"] = "text/html";
            await context.Response.WriteAsync(nameof(HttpStatusCode.Unauthorized));
        }
    }
}
