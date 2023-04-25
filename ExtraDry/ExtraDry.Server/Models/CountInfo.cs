namespace ExtraDry.Server;

/// <summary>
/// Supports the IPartialQueryable ToStatistics method.
/// </summary>
internal class CountInfo {

    public CountInfo(object key, int count)
    {
        Key = key?.ToString() ?? "";
        Count = count;
    }

    public string Key { get; set; }

    public int Count { get; set; }
}
