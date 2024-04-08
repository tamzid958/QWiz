using QWiz.Helpers.Paginator;

namespace QWiz.Helpers.Extensions;

public static class HttpExtension
{
    public static void ConfigureQueryService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IUriService>(o =>
        {
            var accessor = o.GetRequiredService<IHttpContextAccessor>();
            var request = accessor.HttpContext!.Request;
            var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            return new UriService(uri);
        });
    }

    public static void CoRsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(c =>
        {
            c.AddPolicy("AllowOrigin", options =>
            {
                options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public static void CoRsApp(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseCors(options =>
        {
            options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }
}