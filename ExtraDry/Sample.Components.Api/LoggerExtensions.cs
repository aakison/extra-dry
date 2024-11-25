namespace Sample.Components.Api;

internal static partial class LoggerExtensions
{
    [LoggerMessage(
        Message = "Sold {Quantity} of {Description}",
        Level = LogLevel.Information,
        SkipEnabledCheck = true)]
    internal static partial void LogProductSaleDetails(
        this ILogger logger,
        int quantity,
        string description);

    [LoggerMessage(
        Message = "{Message}",
        Level = LogLevel.Information)]
    internal static partial void LogStaticInformation(this ILogger logger, string message);
}
