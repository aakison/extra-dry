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

}
