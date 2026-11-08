namespace ExtraDry.Blazor;

/// <summary>
/// Optional service that stores and lists images for Markdown editors. Register an
/// implementation in DI to give all Markdown editors image upload support without having to
/// wire callbacks on every editor. Explicit callbacks on the <c>Markdown</c> component take
/// precedence over this service.
/// </summary>
public interface IMarkdownImageService
{
    /// <summary>
    /// Stores an image pasted or chosen in the editor, weakly linked to the (nearest) scope, and
    /// returns the image to insert (with a stable URL).
    /// </summary>
    Task<MarkdownImageFile> UploadAsync(IUniqueIdentifier scope, string filename, string contentType, byte[] content);

    /// <summary>
    /// Lists the images available in the gallery for all of the scopes, nearest first.
    /// </summary>
    Task<List<MarkdownImageFile>> ListAsync(IReadOnlyList<IUniqueIdentifier> scopes);
}
