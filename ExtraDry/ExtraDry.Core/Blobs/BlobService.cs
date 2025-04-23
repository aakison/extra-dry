using ExtraDry.Core.Extensions;
using ExtraDry.Core.Internal;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;

namespace ExtraDry.Core;

/// <summary>
/// Registers a API service for Blobs. This service is used to upload files to the server.
/// </summary>
/// <remarks>
/// Create a Blob service with the specified configuration. This service should not be manually
/// added to the IServiceCollection. Instead, use the <see
/// cref="ServiceCollectionExtensions.AddBlobService(Microsoft.Extensions.DependencyInjection.IServiceCollection,
/// Action{BlobServiceOptions})" /> extension method.
/// </remarks>
public class BlobService<TBlob>(
    HttpClient client,
    FileValidationService? fileValidation,
    BlobServiceOptions options,
    ILogger<BlobService<TBlob>> logger)
    where TBlob : IBlob, new()
{
    /// <summary>
    /// Given an entity implementing <see cref="IBlob" />, create a new Blob by calling the
    /// registered Blob endpoint. The URI of the blob will be created from the Blob's UUID.
    /// </summary>
    public async Task CreateAsync(TBlob blob, CancellationToken cancellationToken = default)
    {
        if(blob.Content == null) {
            throw new InvalidOperationException("Blob content must be set before calling CreateAsync.");
        }

        if(string.IsNullOrEmpty(blob.Slug)) {
            var name = fileValidation == null ? blob.Uuid.ToString() : fileValidation.CleanFilename(Path.GetFileNameWithoutExtension(blob.Title));
            blob.Slug = $"{Slug.ToSlug(name)}{Path.GetExtension(blob.Title).ToLowerInvariant()}";
        }

        blob.MD5Hash = options.ValidateHashOnCreate
            ? MD5Core.GetHashString(blob.Content)
            : string.Empty;

        if(options.RewriteWebSafeFilename) {
            blob.Title = fileValidation?.CleanFilename(blob.Title) ?? blob.Title;
        }

        blob.Length = blob.Content.Length;
        using var bytes = BlobSerializer.SerializeBlob(blob);
        var endpoint = ApiEndpoint(blob.Uuid, blob.Slug);
        var response = await client.PostAsync(endpoint, bytes, cancellationToken);
        await response.AssertSuccess(logger);
    }

    /// <summary>
    /// Given a filename, type and content, create a new Blob by calling the registered Blob
    /// endpoint. The URI of the blob will be created from an auto generated UUID.
    /// </summary>
    /// <returns>The Blob that was created locally and corresponds to the remote Blob.</returns>
    public async Task<TBlob> CreateAsync(string filename, string mimeType, byte[] content, CancellationToken cancellationToken = default)
    {
        var blob = new TBlob {
            Title = filename,
            MimeType = mimeType,
            Content = content,
        };
        await CreateAsync(blob, cancellationToken);
        return blob;
    }

    /// <summary>
    /// Given a Blob's UUID, retrieve the Blob from the server. The URI for the blob will not
    /// contain the Blob's filename, so the default filename will be used. This is suitable for use
    /// inside the app, but not ideal for downloading the file.
    /// </summary>
    public async Task<TBlob> ReadAsync(Guid uuid, CancellationToken cancellationToken = default)
    {
        return await ReadAsync(uuid, "unnamed-file", cancellationToken);
    }

    /// <summary>
    /// Given a Blob's UUID, retrieve the Blob from the server. The URI for the blob will contain
    /// the slug provided which improves the URI and allows for downloading files.
    /// </summary>
    public async Task<TBlob> ReadAsync(Guid uuid, string slug, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(uuid, slug);
        var response = await client.GetAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);

        var blob = await BlobSerializer.DeserializeBlobAsync<TBlob>(response, cancellationToken);

        var validator = new DataValidator();
        validator.ValidateObject(blob);
        validator.ThrowIfInvalid();
        return blob;
    }

    public async Task UpdateAsync(TBlob blob, CancellationToken cancellationToken = default)
    {
        if(blob.Content == null) {
            throw new InvalidOperationException("Blob content must be set before calling UpdateAsync.");
        }

        if(string.IsNullOrEmpty(blob.Slug)) {
            var name = fileValidation == null ? blob.Uuid.ToString() : fileValidation.CleanFilename(Path.GetFileNameWithoutExtension(blob.Title));
            blob.Slug = $"{Slug.ToSlug(name)}{Path.GetExtension(blob.Title).ToLowerInvariant()}";
        }

        blob.MD5Hash = options.ValidateHashOnCreate
            ? MD5Core.GetHashString(blob.Content)
            : string.Empty;

        if(options.RewriteWebSafeFilename) {
            blob.Title = fileValidation?.CleanFilename(blob.Title) ?? blob.Title;
        }

        blob.Length = blob.Content.Length;
        using var bytes = BlobSerializer.SerializeBlob(blob);
        var endpoint = ApiEndpoint(blob.Uuid, blob.Slug);
        var response = await client.PutAsync(endpoint, bytes, cancellationToken);
        await response.AssertSuccess(logger);
    }

    public async Task DeleteAsync(Guid uuid, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(uuid);
        var response = await client.DeleteAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
    }

    public long MaxBlobSize => options.MaxBlobSize;

    private string ApiEndpoint(Guid uuid, string filename = "")
    {
        try {
            var url = $"{options.BlobEndpoint}/{uuid}/{filename}".TrimEnd('/');
            return url;
        }
        catch(FormatException ex) {
            throw new DryException("Error occurred connecting to server", $"This is a mis-configuration and not a user error, please see the console output for more information.  Error: {ex.Message}");
        }
    }
}
