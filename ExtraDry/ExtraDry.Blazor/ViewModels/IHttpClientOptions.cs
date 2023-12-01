namespace ExtraDry.Blazor;

internal interface IHttpClientOptions {

    string HttpClientName { get; set; }

    Type? HttpClientType { get; set; }
}
