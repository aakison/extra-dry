namespace ExtraDry.Blazor;

/// <summary>
/// The state of a <see cref="BrowserBlob"/> as it progresses from being selected by the user in
/// the browser through to being uploaded and persisted on the server.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BrowserBlobState
{
    /// <summary>
    /// The file has been selected by the user in the browser, but has not yet been uploaded to
    /// the server. The <see cref="BrowserBlob.File"/> and <see cref="BrowserBlob.Content"/>
    /// properties may be available, but no server-side Blob exists yet.
    /// </summary>
    Pending,

    /// <summary>
    /// The file has been uploaded to the server and a corresponding Blob has been persisted.
    /// </summary>
    Uploaded,
}
