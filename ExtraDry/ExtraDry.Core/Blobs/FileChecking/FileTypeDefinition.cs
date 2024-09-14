namespace ExtraDry.Core;

/// <summary>
/// Definition of a file type that links extensions with mime types and optionally 'magic bytes' 
/// that indicate the file type from the content.  Used to check for valid files during upload.
/// </summary>
public class FileTypeDefinition
{

    /// <inheritdoc cref="FileTypeDefinition" />
    public FileTypeDefinition() { }

    /// <inheritdoc cref="FileTypeDefinition" />
    public FileTypeDefinition(string extension, string mimeType, string description = "")
    {
        Extensions.Add(extension);
        MimeTypes.Add(mimeType);
        Description = description;
    }

    /// <summary>
    /// A human readable description of the file type, e.g. "Portable Network Graphics Image".
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// The file extensions that are associated with this file type, e.g. ["jpg", "jpeg"].
    /// </summary>
    public List<string> Extensions { get; set; } = [];

    /// <summary>
    /// The mime types that are associated with this file type, e.g. ["application/xml"].
    /// </summary>
    public List<string> MimeTypes { get; set; } = [];

    /// <summary>
    /// An object defining a way that this file type can be identified by its content.
    /// </summary>
    public List<MagicBytes> MagicBytes { get; set; } = [];
}
