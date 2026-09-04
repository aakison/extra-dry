using ExtraDry.Core.Internal;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json.Serialization;

namespace ExtraDry.Blazor;

/// <summary>
/// Represents a file that has been selected by the user in the browser for a
/// <see cref="Components.FileField"/> or <see cref="Components.ImageField"/>, but which may not
/// yet have been uploaded to the server. Implements <see cref="IBlob"/> so that it can be stored
/// and serialized alongside other field values (e.g. in a form response's value dictionary)
/// using the same shape as a server-persisted Blob, before and after upload.
/// </summary>
/// <remarks>
/// The <see cref="File"/> and <see cref="Content"/> properties are only meaningful in the browser
/// and are excluded from JSON serialization, so that a partially completed form (with a file
/// selected but not yet uploaded) can still be saved and reloaded without error. Once the file has
/// been uploaded, the consuming application should update <see cref="State"/> to
/// <see cref="BrowserBlobState.Uploaded"/> and clear <see cref="File"/>/<see cref="Content"/>, or
/// replace the value with the resulting server-side Blob altogether.
/// </remarks>
/// <remarks>
/// Creates a <see cref="BrowserBlob"/> representing a file selected by the user, populating
/// metadata from the <see cref="IBrowserFile"/> immediately so it is available for display
/// even before the file's content has been read or uploaded.
/// </remarks>
public class BrowserBlob(IBrowserFile file) : IBlob
{
    /// <inheritdoc/>
    public Guid Uuid { get; set; } = Guid.CreateVersion7();

    /// <summary>
    /// The title of the Blob, which is the original filename as selected by the user. This is
    /// typically unsafe for web use; see <see cref="Slug"/> for a URL-safe reference.
    /// </summary>
    public string Title { get; set; } = file.Name;

    /// <summary>
    /// A URL-safe reference derived from <see cref="Title"/>. Some client operating systems allow
    /// filename characters that are not valid in a URI, so the filename is converted to a slug
    /// for use as an actual resource reference.
    /// </summary>
    public string Slug { get; set; } = ToFilenameSlug(file.Name);

    /// <inheritdoc/>
    public string MimeType { get; set; } = file.ContentType;

    /// <inheritdoc/>
    public string MD5Hash { get; set; } = "";

    /// <inheritdoc/>
    public int Length { get; set; } = (int)file.Size;

    /// <summary>
    /// The state of this file, indicating whether it is still pending upload or has already been
    /// uploaded to the server. This is the single source of truth for determining whether this
    /// Blob still needs to be uploaded; consumers should check this property rather than checking
    /// <see cref="File"/> or <see cref="Content"/> for null.
    /// </summary>
    public BrowserBlobState State { get; set; } = BrowserBlobState.Pending;

    /// <inheritdoc/>
    /// <remarks>
    /// Excluded from JSON serialization since content is only relevant while pending upload in the
    /// browser and could be large; it is not appropriate to round-trip through a saved form
    /// response.
    /// </remarks>
    [JsonIgnore]
    public byte[]? Content { get; set; }

    /// <summary>
    /// The browser file handle for the selected file, as provided by an <c>InputFile</c> change
    /// event. Only available in the browser while the file is pending upload; excluded from JSON
    /// serialization since it has no meaningful representation on the server.
    /// </summary>
    [JsonIgnore]
    public IBrowserFile? File { get; set; } = file;

    /// <summary>
    /// Converts a filename into a URI-safe slug that retains its extension, e.g.
    /// <c>"My Statement (2).pdf"</c> becomes <c>"my-statement-2.pdf"</c>. A plain
    /// <see cref="ExtraDry.Core.Slug.ToSlug(string, bool)"/> would strip the separating dot along
    /// with other punctuation, leaving the server unable to determine the file's extension.
    /// </summary>
    private static string ToFilenameSlug(string filename)
    {
        var extension = Path.GetExtension(filename).TrimStart('.');
        var name = Path.GetFileNameWithoutExtension(filename);
        var slug = ExtraDry.Core.Slug.ToSlug(name);
        return extension == "" ? slug : $"{slug}.{ExtraDry.Core.Slug.ToSlug(extension)}";
    }

    /// <summary>
    /// Reads the content of <see cref="File"/> into memory, populating <see cref="Content"/> and
    /// computing <see cref="MD5Hash"/>. Must be called before the file is uploaded.
    /// </summary>
    /// <param name="maxAllowedSize">The maximum number of bytes to read from the file.</param>
    public async Task ReadContentAsync(long maxAllowedSize)
    {
        if(Content is not null) {
            // Already read, e.g. by a preview. Reading `File` a second time is unreliable since
            // the browser's underlying file handle may no longer be resolvable by the JS runtime
            // once it has already been streamed once.
            return;
        }
        if(File is null) {
            return;
        }
        using var stream = File.OpenReadStream(maxAllowedSize);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        Content = memoryStream.ToArray();
        Length = Content.Length;
        MD5Hash = MD5Core.GetHashString(Content);
    }
}
