using Microsoft.OpenApi.Models;

namespace QWiz.Helpers.Security;

public class OpenApiOAuthSecurityScheme : OpenApiSecurityScheme
{
    public OpenApiOAuthSecurityScheme(string domain, string audience)
    {
        Type = SecuritySchemeType.OAuth2;
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"https://{domain}/authorize"),
                TokenUrl = new Uri($"https://{domain}/oauth/token")
            }
        };
    }
}