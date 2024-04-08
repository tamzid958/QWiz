using Microsoft.OpenApi.Models;

namespace QWiz.Helpers.Security;

public class OpenApiOAuthSecurityRequirement : OpenApiSecurityRequirement
{
    public OpenApiOAuthSecurityRequirement()
    {
        Add(
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "oauth2",
                In = ParameterLocation.Header
            },
            new List<string>()
        );
    }
}