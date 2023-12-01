using ExtraDry.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

namespace ExtraDry.Blazor;

public class BlobService
{
    /// <summary>
    /// Create a Blob service with the specified configuration.  This service should not be 
    /// manually added to the IServiceCollection.  Instead, use the <see cref="ServiceCollectionExtensions.AddCrudService{T}(Microsoft.Extensions.DependencyInjection.IServiceCollection, Action{CrudServiceOptions})" />
    /// extension method.
    /// </summary>
    public BlobService(HttpClient client, BlobServiceOptions options, ILogger<BlobService> logger)
    {
        http = client;
        Options = options;
        this.logger = logger;
    }

    public async Task UploadAsync(BlobInfo item, byte[] content, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(item);
        using var requestContent = new ByteArrayContent(content);
        var endpoint = ApiEndpoint(nameof(UploadAsync), item.Uuid);
        logger.LogEndpointCall(typeof(BlobInfo), endpoint);
        requestContent.Headers.Add(BlobInfo.MimeTypeHeaderName, item.MimeType);
        requestContent.Headers.Add(BlobInfo.SlugHeaderName, item.Slug);
        requestContent.Headers.Add(BlobInfo.TitleHeaderName, item.Title);
        requestContent.Headers.Add(BlobInfo.ScopeHeaderName, item.Scope.ToString());
        requestContent.Headers.Add(BlobInfo.UuidHeaderName, item.Uuid.ToString());
        requestContent.Headers.Add(BlobInfo.ShaHashHeaderName, item.ShaHash);
        var response = await http.PostAsync(endpoint, requestContent, cancellationToken);
        await response.AssertSuccess(logger);
    }

    public async Task UploadAsync(string filename, string mimeType, byte[] content, CancellationToken cancellationToken = default)
    {
        var item = new BlobInfo {
            Slug = filename,
            Title = filename,
            MimeType = mimeType,
            //Scope = Scope,
            Uuid = Guid.NewGuid(),
            //ShaHash = ShaHash(content),
        };
        await UploadAsync(item, content, cancellationToken);
    }

    public async Task UploadAsync(IBrowserFile file, CancellationToken cancellationToken = default)
    {
        var item = new BlobInfo {
            Slug = Guid.NewGuid().ToString().ToLowerInvariant(),
            Title = file.Name,
            MimeType = file.ContentType,
            Scope = BlobScope.Public,
            Uuid = Guid.NewGuid(),
        };
        // TODO: How to manage maximum stream size...?
        using var streamContent = new StreamContent(file.OpenReadStream(10_000_000, cancellationToken));
        await UploadAsync(item, streamContent, cancellationToken);
    }

    public async Task UploadAsync(BlobInfo item, HttpContent content, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(item);
        var endpoint = ApiEndpoint(nameof(UploadAsync), item.Uuid);
        logger.LogEndpointCall(typeof(BlobInfo), endpoint);
        content.Headers.Add("Content-Type", item.MimeType);
        content.Headers.Add(BlobInfo.SlugHeaderName, item.Slug);
        content.Headers.Add(BlobInfo.TitleHeaderName, item.Title);
        content.Headers.Add(BlobInfo.ScopeHeaderName, item.Scope.ToString());
        content.Headers.Add(BlobInfo.UuidHeaderName, item.Uuid.ToString());
        content.Headers.Add(BlobInfo.MimeTypeHeaderName, item.MimeType);
        //content.Headers.Add(BlobInfo.ShaHashHeaderName, item.ShaHash);
        var response = await http.PostAsync(endpoint, content, cancellationToken);
        await response.AssertSuccess(logger);
    }

    private string ApiEndpoint(string method, object key, params object[] args)
    {
        try {
            var baseUrl = string.Format(CultureInfo.InvariantCulture, Options.BlobEndpoint, args);
            var url = $"{baseUrl}/{key}".TrimEnd('/');
            return url;
        }
        catch(FormatException ex) {
            var argsFormatted = string.Join(',', args?.Select(e => e?.ToString()) ?? Array.Empty<string>());
            logger.LogFormattingError(typeof(BlobInfo), Options.BlobEndpoint, argsFormatted, ex, method);
            throw new DryException("Error occurred connecting to server", "This is a mis-configuration and not a user error, please see the console output for more information.");
        }
    }

    private BlobServiceOptions Options { get; }

    private readonly HttpClient http;

    private readonly ILogger<BlobService> logger;
}
