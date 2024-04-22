using QWiz.Services;

namespace QWiz.Helpers.Extensions;

public static class ServiceExtension
{
    public static void ServiceDependencyInjection(this IServiceCollection service)
    {
        service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        service.AddScoped<FileExtension>();
        service.AddScoped<AppUserService>();
        service.AddScoped<CategoryService>();
        service.AddScoped<QuestionService>();
        service.AddScoped<ApproverService>();
        service.AddScoped<ApprovalLogService>();
    }
}