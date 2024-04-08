using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QWiz.Helpers.Exception;
using QWiz.Helpers.Extensions;
using QWiz.Helpers.Security;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = "wwwroot"
});

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddRouting();

builder.Services.ConfigureDbConnection(builder.Configuration);

builder.Services.ConfigureIdentityUser();

builder.Services.ServiceDependencyInjection();

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureJwtAuthentication(builder.Configuration);

builder.Services.ConfigureQueryService();


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
}).AddJsonOptions(opts => { opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true; });

builder.Services.CoRsConfiguration(builder.Configuration);

builder.Services.AddDirectoryBrowser();

builder.Services.AddHealthChecks();

builder.Services.AddResponseCompression();

builder.Services.AddResponseCaching();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    OpenApiSecurityScheme securityScheme = new OpenApiBearerSecurityScheme();
    OpenApiSecurityRequirement securityRequirement = new OpenApiBearerSecurityRequirements(securityScheme);

    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Logging.ClearProviders().AddConsole();

builder.Services.LoggingDependencyInjection();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.EnableFilter();
    c.ConfigObject.DocExpansion = DocExpansion.List;
});

app.UseReDoc(c =>
{
    c.DocumentTitle = "RE-DOC API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
});


app.UseDirectoryBrowser();

app.UseFileServer(true);

app.UseHealthChecks("/health");

app.UseResponseCompression();

app.UseResponseCaching();

app.UseStaticFiles();

app.UseRouting();

app.CoRsApp(builder.Configuration);

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();