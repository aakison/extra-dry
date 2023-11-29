namespace ExtraDry.Core;

/// <summary>
/// An entity representing an item in the file blacklist. It contains its file extension and how to check the file.
/// </summary>
public class BlacklistFileType {
    /// <summary>
    /// The file extension to blacklist
    /// </summary>
    public string Extension { get; set; } = string.Empty;

    /// <summary>
    /// The method to check the file extension. By default will check both filename and magic bytes.
    /// </summary>
    public CheckType CheckType { get; set; } = CheckType.BytesAndFilename;
}
