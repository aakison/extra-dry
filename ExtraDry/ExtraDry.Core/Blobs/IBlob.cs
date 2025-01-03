namespace ExtraDry.Core;

/// <summary>
/// The interface for Extra Dry support for Blobs. Use the <see cref="Blob" /> class for simple
/// scenarios, or expand the metadata for Blobs with your own class that implements this interface.
/// </summary>
public interface IBlob : IResourceIdentifiers
{
    /// <summary>
    /// The content of the Blob. This is the actual file data. This allows for nulls to support
    /// HEAD requests.
    /// </summary>
    byte[]? Content { get; set; }

    /// <summary>
    /// The length of the content of the Blob in bytes. If the Content is null, this is the length
    /// of the blob on the remote server, i.e. as the result of a HEAD request.
    /// </summary>
    int Length { get; set; }

    /// <summary>
    /// The MIME Type (aka Content-Type) of the Blob. This is declared when the Blob is created, no
    /// attempt to dynamically determine the type is made.
    /// </summary>
    string MimeType { get; set; }

    /// <summary>
    /// The optional MD5 hash of the content of the Blob. This is used to verify the integrity of
    /// files during upload and download. If the hash is not provided, the integrity of the file is
    /// not checked.
    /// </summary>
    string MD5Hash { get; set; }
}
