namespace ExtraDry.Core;

public class UploadConfiguration {
    /// <summary>
    /// The whitelist of file extensions that you would like your application to allow
    /// </summary>
    public List<string> ExtensionWhitelist { get; set; } = new List<string>();

    /// <summary>
    /// Sets a value indicating whether the file service should be checking magic bytes and mime types.
    /// Set to true for server-side validation, and false for client side.
    /// </summary>
    public bool CheckMagicBytesAndMimes { get; set; }

    /// <summary>
    /// If <see cref="CheckMagicBytesAndMimes"/> is set to true, you can optionally set a location for a file database. If left blank, the default will be used.
    /// </summary>
    public string? FileDatabaseLocation { get; set; }

    /// <summary>
    /// A list of extensions that will form the blacklist of file types that you would like to exclude from your system.
    /// Leave empty to use a default set
    /// </summary>
    public List<BlacklistFileType> ExtensionBlacklist { get; set; } = new List<BlacklistFileType>();
}

/// <summary>
/// An entity representing an item in the file blacklist. It contains its file extension and how to check the file.
/// </summary>
public class BlacklistFileType {
    /// <summary>
    /// The file extension to blacklist
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// The method to check the file extension. By default will check both filename and magic bytes.
    /// </summary>
    public CheckType CheckType { get; set; } = CheckType.BytesAndFilename;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CheckType
{
    /// <summary>
    /// For this extension, check the filename as well as the magic bytes. This is the default behaviour
    /// </summary>
    BytesAndFilename,

    /// <summary>
    /// For this extension, don't reject based off magic bytes. 
    /// </summary>
    /// <remarks>Used where a shared magic byte would reject a file you want. Eg Reject .apk, but don't reject .docx</remarks>
    FilenameOnly
}
