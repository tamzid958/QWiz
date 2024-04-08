using System.Runtime.CompilerServices;

namespace QWiz.Helpers.Extensions;

public static class LoggerExtension
{
    public static void Log(this ILogger logger,
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0,
        LogLevel logLevel = LogLevel.Trace)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{filePath}:{lineNumber} -----> {message}");

        logger.Log(
            logLevel,
            "{FilePath}:{LineNumber} -----> {Message}",
            filePath,
            lineNumber, message
        );
    }

    public static void LoggingDependencyInjection(this IServiceCollection services)
    {
    }
}