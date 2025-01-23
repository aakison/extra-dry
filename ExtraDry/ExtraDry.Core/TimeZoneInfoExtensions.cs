namespace ExtraDry.Core;

public static class TimeZoneInfoExtensions
{
    public static string DisplayCode(this TimeZoneInfo timeZone)
    {
        var code = timeZone.Id;
        if(code.StartsWith("UTC", StringComparison.InvariantCulture)) {
            return code;
        }
        var offset = timeZone.BaseUtcOffset;
        var sign = offset.TotalHours > 0 ? "+" : "";
        return $"{code} ({sign}{offset.TotalHours})";
    }
}
