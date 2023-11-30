namespace ExtraDry.Blazor;

using ExtraDry.Core.Models;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

internal static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Debug, EventId = 10,
        Message = "{Method} info: {Message}")]
    internal static partial void LogConsoleVerbose(this ILogger logger, string message, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Debug, EventId = 11,
    Message = "{Method} loading SVG icon {IconKey} from {IconImagePath}")]
    internal static partial void LogLoadingIcon(this ILogger logger, string iconKey, string iconImagePath, [CallerMemberName] string? method = null);


    [LoggerMessage(Level = LogLevel.Information, EventId = 20,
        Message = "{Method} for type {EntityType} on endpoint {Endpoint}")]
    internal static partial void LogEndpointCall(this ILogger logger, Type entityType, string endpoint, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Information, EventId = 21,
        Message = "{Method} for type {EntityType} on endpoint {Endpoint} returned {Body}")]
    internal static partial void LogEndpointResult(this ILogger logger, Type entityType, string endpoint, string? body, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Information, EventId = 22,
        Message = "{Method} for type {EntityType} returned {Count}/{Total} records from position {Start}")]
    internal static partial void LogPartialResults(this ILogger logger, Type entityType, int start, int count, int total, [CallerMemberName] string? method = null);


    [LoggerMessage(Level = LogLevel.Warning, EventId = 40,
    Message = "{Method} warning: {Message}")]
    internal static partial void LogConsoleWarning(this ILogger logger, string message, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Warning, EventId = 41,
    Message = "{Method} warning: Theme already contains an icon with key {Icon}, skipping.  Remove duplicate IconInfo from Theme's Icon collection.")]
    internal static partial void LogDuplicateIcon(this ILogger logger, string icon, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Warning, EventId = 42,
    Message = "{Method} warning: Icon '{Icon}' not registered, add an entry for icon to the `Icons` attribute of the `Theme` component.")]
    internal static partial void LogMissingIcon(this ILogger logger, string icon, [CallerMemberName] string? method = null);


    [LoggerMessage(Level = LogLevel.Error, EventId = 50,
        Message = "{Method} error: {Message}")]
    internal static partial void LogConsoleError(this ILogger logger, string message, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Error, EventId = 51,
    Message = "{Method} error: {Message}")]
    internal static partial void LogConsoleError(this ILogger logger, string message, Exception exception, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Error, EventId = 52,
        Message = "Formatting problem while constructing endpoint for {Method} on {EntityType}.  Typically the endpoint provided has additional placeholders that have not been provided. The endpoint template ({EndpointTemplate}), could not be satisfied with arguments ({Args}).")]
    internal static partial void LogFormattingError(this ILogger logger, Type entityType, string endpointTemplate, string args, Exception ex,  [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Error, EventId = 53,
        Message = "{Method} failed to load icon {IconKey} from {IconImagePath}")]
    internal static partial void LogIconFailed(this ILogger logger, string? iconKey, string? iconImagePath, Exception ex, [CallerMemberName] string? method = null);

    [LoggerMessage(Level = LogLevel.Error, EventId = 54,
        Message = "{Method} HTTP request failed code {status}, type {Type} at {Instance}; {details}")]
    internal static partial void LogProblemDetails(this ILogger logger, int status, string type, string instance, string details, [CallerMemberName] string? method = null);

    internal static void LogProblemDetails(this ILogger logger, ProblemDetails problem, [CallerMemberName] string? method = null)
        => LogProblemDetails(logger, problem.Status ?? 0, problem.Type ?? "unknown", problem.Instance ?? "unknown", problem.Detail ?? "", method);


    [LoggerMessage(Level = LogLevel.Error, EventId = 55,
        Message = "{Method} No option provider was registered.  An attempt to display a DryInput for type {PropertyType}, but no option provider was registered.  To enable select functionality for linked types, please add a scoped reference to the `IOptionProvider` in `Main`.  E.g. `builder.Services.AddScoped<IOptionProvider<{PropertyType}>>(e => new MyOptionProvider());`.  Also note that IListService implements IOptionProvider and can be used to register RESTful APIs")]
    internal static partial void LogMissingOptionProvider(this ILogger logger, string propertyType, [CallerMemberName] string? method = null);
    

}
