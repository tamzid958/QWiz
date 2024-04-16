using QWiz.Services;

namespace QWiz.Helpers.Extensions;

public static class ServiceExtension
{
    public static void ServiceDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped<FileExtension>();
        service.AddScoped<AppUserService>();
        service.AddScoped<CategoryService>();
        service.AddScoped<ApproverService>();
        service.AddScoped<QuestionService>();
    }
}