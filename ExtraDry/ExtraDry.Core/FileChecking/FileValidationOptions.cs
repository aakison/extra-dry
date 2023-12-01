namespace ExtraDry.Core;

/// <summary>
/// Options for the file validator service. This is used to configure the file validator service.
/// </summary>
public class FileValidationOptions
{

    /// <summary>
    /// The whitelist of file extensions that you would like your application to allow
    /// </summary>
    public List<string> ExtensionWhitelist { get; set; } = new() {
        "7z", "3ds", "3gp", "ai", "avi", "bak", "bmp", "bz2", "csv", "cxp", "doc", "docx", "dot", 
        "dwg", "dwt", "dxf", "eml", "fpx", "gif", "gz", "heic", "html", "iam", "idw", "ifc", "iges", 
        "ipt", "jpeg", "jpg", "key", "log", "m4a", "m4v", "md", "mht", "mov", "mp3", "mp4", 
        "mpeg", "mpg", "msg", "nwc", "nwd", "nwf", "obj", "ods", "odt", "oft", "ogg", "pdf", "png", 
        "ppt", "pptm", "pptx", "psd", "rar", "rec", "rfa", "rte", "rtf", "rvt", "shp", "stl", "tar", 
        "tgz", "tif", "tiff", "ttd", "txt", "wav", "xls", "xlsm", "xlsx", "xltm", "zip"
    };

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

    /// <summary>
    /// Determines if the <see cref="FileValidator" /> validates the file content.  The default is 
    /// <see cref="ValidationCondition.ServerSide" /> which runs content validation rules on the 
    /// server, but not on the blazor client.  This is recommended as the content validation rules
    /// requires a database of file signatures and mime types that could cause performance issues 
    /// if downloaded to the client.
    /// </summary>
    public ValidationCondition ValidateContent { get; set; } = ValidationCondition.ServerSide;

    /// <summary>
    /// Determines if the <see cref="FileValidator" /> validates file extensions.  The default is
    /// <see cref="ValidationCondition.Always" /> which checks extension on both the server and the
    /// client.  This is recommended as it is a simple check that can be performed on the client.
    /// </summary>
    public ValidationCondition ValidateExtension { get; set; } = ValidationCondition.Always;

    /// <summary>
    /// Determines if the <see cref="FileValidator" /> validates filenames.  The default is
    /// <see cref="ValidationCondition.Always" /> which checks filenames on both the server and the
    /// client.  This is recommended as it is a simple check that can be performed on the client.
    /// </summary>
    public ValidationCondition ValidateFilename { get; set; } = ValidationCondition.Always;

}
