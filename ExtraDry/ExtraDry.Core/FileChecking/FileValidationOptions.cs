namespace ExtraDry.Core;

/// <summary>
/// Options for the file validator service. This is used to configure the file validator service.
/// </summary>
public class FileValidationOptions
{
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
