using ExtraDry.Blazor.Extensions;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ExtraDry.Blazor;

public interface IBlobService {
    //Task<ICollection<IBlob>> ListAsync(string prefix, Dictionary<string, object> metaDataFilter = null);
    Task<IBlobInfo> CreateAsync(byte[] content, string filename = "not-specified", Dictionary<string, object?>? metaData = null);
    //Task<IBlob> RetrieveAsync(string filename);
    //Task<IBlob> UpdateAsync(IBlob exemplar);
    //Task DeleteAsync(string filename);

    //Task<Stream> OpenBlob(IBlob blob);
    //Task<Stream> OpenBlob(string filename);
}

/// <summary>
/// A default implementation of IBlobService for
/// </summary>
public class DryBlobService : IBlobService {

    public DryBlobService(HttpClient client, string entityEndpointTemplate, ILogger<DryBlobService> iLogger)
    {
        http = client;
        ApiTemplate = entityEndpointTemplate;
        logger = iLogger;
    }

    public string ApiTemplate { get; set; }

    public BlobScope Scope { get; set; }

    public async Task<IBlobInfo> CreateAsync(byte[] content, string filename = "not-specified", Dictionary<string, object?>? metaData = null)
    {
        // TODO: Map DryException on server to DryException in Blazor for better messaging...
        using var contentToUpload = new ByteArrayContent(content);
        contentToUpload.Headers.Add("content-type", "application/octet-stream");
        var endpoint = ApiEndpoint("POST", Scope.ToString().ToLowerInvariant(), filename ?? string.Empty);
        try {
            var response = await http.PostAsync(endpoint, contentToUpload);
            await response.AssertSuccess(logger);
            var item = await response.Content.ReadFromJsonAsync<BlobInfo>() 
                ?? throw new DryException("Blob created endpoint did not return a Blob.", "Unable to upload image. 0x0F2D6C00");
            return item;
        }
        catch(Exception ex) {
            Console.Error.WriteLine($"Server Side Error while processing CrudService.CreateAsync.\n" +
                $"API Endpoint: POST {endpoint}\n" +
                $"Error Response: {ex.Message}");
            throw;
        }
    }

    private string ApiEndpoint(string method, object key, params object[] args)
    {
        try {
            var formatArgs = new List<object>(args);
            formatArgs.Insert(0, key);
            args = formatArgs.ToArray();
            return string.Format(CultureInfo.InvariantCulture, ApiTemplate, args).TrimEnd('/');
        }
        catch(FormatException ex) {
            var argsFormatted = string.Join(',', args?.Select(e => e?.ToString()) ?? Array.Empty<string>());
            var message = $"Formatting problem while constructing endpoint for `DryBlobService.{method}`.  Typically the endpoint provided has additional placeholders that have not been provided. The endpoint template ({ApiTemplate}), could not be satisifed with arguments ({argsFormatted}).  Inner Exception was:  {ex.Message}";
            Console.Error.WriteLine(message);
            throw new DryException(message, "Error occurred connecting to server.");
        }
    }

    private readonly HttpClient http;

    private readonly ILogger<DryBlobService> logger;

}
