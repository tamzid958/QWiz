using Microsoft.OpenApi.Models;

namespace QWiz.Helpers.Security;

public class OpenApiBearerSecurityScheme : OpenApiSecurityScheme
{
    public OpenApiBearerSecurityScheme()
    {
        Description = "Using the Authorization header with the bearer scheme.";
        Name = "Authorization";
        In = ParameterLocation.Header;
        Type = SecuritySchemeType.Http;
        Scheme = "bearer";
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        };
    }
}