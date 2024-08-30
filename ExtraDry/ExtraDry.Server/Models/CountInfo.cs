namespace ExtraDry.Server;

/// <summary>
/// Supports the IPartialQueryable ToStatistics method.
/// </summary>
internal class CountInfo(
    object key, 
    int count)
{
    public string Key { get; set; } = key?.ToString() ?? "";

    public int Count { get; set; } = count;
}
