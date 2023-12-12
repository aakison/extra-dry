namespace ExtraDry.Core;

/// <summary>
/// Defines a file type that can be checked for when uploading a file.
/// </summary>
public class FileTypeDefinition
{
    /// <summary>
    /// A human readable description of the file type eg. "Portable Network Graphics Image"
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The file extensions that are associated with this file type. eg ["jpg", "jpeg"]
    /// </summary>
    public List<string> Extensions { get; set; } = new List<string>();

    /// <summary>
    /// The mime types that are associated with this file type. eg ["application/xml"]
    /// </summary>
    public List<string> MimeTypes { get; set; } = new List<string>();

    /// <summary>
    /// An object defining a way that this file type can be identified by its content.
    /// </summary>
    public List<MagicBytes> MagicBytes { get; set; } = new List<MagicBytes>();
}
