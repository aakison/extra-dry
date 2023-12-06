namespace ExtraDry.Blazor;

/// <summary>
/// Options for the Blob service.
/// </summary>
internal interface IBlobServiceOptions : IHttpClientOptions {
    
    /// <summary>
    /// Set the maximum size of a blob that can be uploaded.  This is a security measure to prevent
    /// denial of service attacks.  The default is 10MB.
    /// </summary>
    int MaxBlobSize { get; }

    /// <summary>
    /// Indicates if the client should compute a content hash and send it to the server.  The 
    /// server will use this hash to validate the content.  This is a reliability measure to ensure
    /// content is not corrupted in transit.  However, it does require additional memory and 
    /// processing time on the client and on the server.  The default is true.
    /// </summary>
    bool ValidateHashOnCreate { get; }
}
