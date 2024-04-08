using Microsoft.OpenApi.Models;

namespace QWiz.Helpers.Security;

public class OpenApiBearerSecurityRequirements : OpenApiSecurityRequirement
{
    public OpenApiBearerSecurityRequirements(OpenApiSecurityScheme scheme)
    {
        Add(scheme, new[] { "Bearer" });
    }
}