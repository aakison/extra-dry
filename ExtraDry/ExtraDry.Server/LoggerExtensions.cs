namespace ExtraDry.Server;

using ExtraDry.Core.Models;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

internal static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Debug, EventId = 10,
        Message = "{Method} info: {Message}")]
    internal static partial void LogTextVerbose(this ILogger logger, string message, [CallerMemberName] string? method = null);


    [LoggerMessage(Level = LogLevel.Information, EventId = 20,
        Message = "{Method} info: {Message}")]
    internal static partial void LogTextInfo(this ILogger logger, string message, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Information, EventId = 21,
        Message = "{Method} info: Table {Table} changed; {Message}")]
    internal static partial void LogTableChange(this ILogger logger, string message, string table, [CallerMemberName] string? method = null);


    [LoggerMessage(Level = LogLevel.Warning, EventId = 40,
        Message = "{Method} warning: {Message}")]
    internal static partial void LogTextWarning(this ILogger logger, string message, [CallerMemberName] string? method = null);


    [LoggerMessage(Level = LogLevel.Error, EventId = 50,
        Message = "{Method} error: {Message}")]
    internal static partial void LogTextError(this ILogger logger, string message, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Error, EventId = 54,
        Message = "{Method} HTTP request failed code {status}, type {Type} at {Instance}; {details}")]
    internal static partial void LogProblemDetails(this ILogger logger, int status, string type, string instance, string details, [CallerMemberName] string? method = null);

    internal static void LogProblemDetails(this ILogger logger, ProblemDetails problem, [CallerMemberName] string? method = null)
        => LogProblemDetails(logger, problem.Status ?? 0, problem.Type ?? "unknown", problem.Instance ?? "unknown", problem.Detail ?? "", method);

    [LoggerMessage(Level = LogLevel.Information, EventId = 60, 
Message = "Cron job '{Name}' triggered at {Time}")]
    internal static partial void LogCronJobTrigger(this ILogger logger, string name, string time);

    [LoggerMessage(Level = LogLevel.Information, EventId = 61, Message = "Next Cron Job Occurrence (UTC): {Time}")]
    internal static partial void LogCronJobNextInfo(this ILogger logger, string time);

    [LoggerMessage(Level = LogLevel.Information, EventId = 62, Message = "Cron Service Stopped")]
    internal static partial void LogCronServiceStopped(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, EventId = 63, Message = "Server Time (UTC): {Time}")]
    internal static partial void LogServerTime(this ILogger logger, string time);

    [LoggerMessage(Level = LogLevel.Information, EventId = 64, Message = "Cron Service Host Started with {JobCount} jobs")]
    internal static partial void LogCronServiceStarted(this ILogger logger, int jobCount);

    [LoggerMessage(Level = LogLevel.Information, EventId = 65, Message = "Job '{JobName}' with schedule '{Schedule}' next 3 occurrences:\n\t{Time1}\n\t{Time2}\n\t{Time3}")]
    internal static partial void LogCronJobNexts(this ILogger logger, string JobName, string schedule, string time1, string time2, string time3);


    [LoggerMessage(Level = LogLevel.Information, EventId = 70, Message = "Resolved Configuration for '{Name}':\n\t{List}")]
    internal static partial void LogConfigurationList(this ILogger logger, string name, string list);

    [LoggerMessage(Level = LogLevel.Warning, EventId = 71, Message = "Configuration Failed Validation:\n\t{Results}")]
    internal static partial void LogConfigurationValidationError(this ILogger logger, string results);


}
