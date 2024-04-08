using System.Data;
using System.Net;
using System.Text.Json;

namespace QWiz.Helpers.Exception;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                AppException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                BadHttpRequestException => (int)HttpStatusCode.BadRequest,
                DuplicateNameException => (int)HttpStatusCode.Ambiguous,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(new ExceptionMessage
            {
                Code = response.StatusCode,
                Message = error.Message.Contains(": ") && error.Message.Contains("at")
                    ? error.Message.Split([": "], StringSplitOptions.None)[1]
                        .Split("at")[0]
                        .Trim()
                    : error.Message
            });

            await response.WriteAsync(result);
        }
    }
}