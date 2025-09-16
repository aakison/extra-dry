using Microsoft.AspNetCore.Components.Forms;

namespace ExtraDry.Blazor;

public static class BlobClientExtensions
{
    /// <summary>
    /// Given a <see cref="IBrowserFile" /> from an <see cref="InputFile" />, create a new Blob by
    /// calling the registered Blob endpoint. The URI of the blob will be created from an auto
    /// generated UUID.
    /// </summary>
    /// <returns>The Blob that was created locally and corresponds to the remote Blob.</returns>
    public static async Task<TBlob> CreateAsync<TBlob>(this BlobClient<TBlob> blobs, IBrowserFile file, CancellationToken cancellationToken = default) where TBlob : IBlob, new()
    {
        var blob = new TBlob {
            Title = file.Name,
            MimeType = file.ContentType,
            Length = (int)file.Size,
        };
        var memoryStream = new MemoryStream();
        using var stream = file.OpenReadStream(blobs.MaxBlobSize, cancellationToken);
        await stream.CopyToAsync(memoryStream, cancellationToken);
        blob.Content = memoryStream.ToArray();
        await blobs.CreateAsync(blob, cancellationToken);
        return blob;
    }
}
