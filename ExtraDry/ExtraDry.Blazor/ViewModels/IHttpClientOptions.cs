namespace ExtraDry.Blazor;

/// <summary>
/// Base interface for options that configure an HttpClient.
/// </summary>
internal interface IHttpClientOptions
{
    /// <summary>
    /// The name of the HttpClient to use for the Blob service. This is optional, if not specified
    /// the default HttpClient will be used.
    /// </summary>
    string HttpClientName { get; }

    /// <summary>
    /// The type of the HttpClient to use for the Blob service. This is optional, if not specified
    /// the default HttpClient will be used. This property is mutually exclusive with
    /// HttpClientName.
    /// </summary>
    Type? HttpClientType { get; }
}
