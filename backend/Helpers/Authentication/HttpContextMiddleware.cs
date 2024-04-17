using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using QWiz.Entities;

namespace QWiz.Helpers.Authentication;

public class HttpContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, UserManager<AppUser> userManager)
    {
        if (httpContext.User.Identity is { IsAuthenticated: true })
        {
            var appUser = userManager.Users.First(o => o.UserName == httpContext.User.Identity.Name);
            var claims = new List<Claim>
            {
                new("UserId", appUser.Id)
            };

            var appIdentity = new ClaimsIdentity(claims);
            httpContext.User.AddIdentity(appIdentity);
        }

        await next(httpContext);
    }
}