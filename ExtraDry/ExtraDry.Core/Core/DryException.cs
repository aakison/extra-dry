using ExtraDry.Core.Models;
using System.Net;

namespace ExtraDry.Core;

/// <summary>
/// A generic exception for applications to use that enables passing additional problem details
/// from the server through the API to the client using RFC7807.
/// </summary>
[Serializable]
public sealed class DryException : Exception {

    /// <summary>
    /// Construct an empty exception, prefer use of a constructor with more information.
    /// </summary>
    public DryException() { }

    public DryException(ProblemDetails? details) : base(details?.Title)
    {
        if(details != null) {
            ProblemDetails = details;
        }
    }

    public DryException(HttpStatusCode status, string message, string userMessage) : base(message)
    {
        ProblemDetails.Status = (int)status;
        ProblemDetails.Title = message;
        ProblemDetails.Detail = userMessage;
    }

    public DryException(string message) : base(message) {
        ProblemDetails.Title = message;
    }

    public DryException(string message, Exception inner) : base(message, inner) {
        ProblemDetails.Title = message;
    }

    public DryException(string message, string userMessage) : base(message)
    {
        ProblemDetails.Title = message;
        ProblemDetails.Detail = userMessage;
    }

    /// <summary>
    /// If available, an exception message that is suitable to show to users.
    /// E.g. certain validation exceptions can be shown, but null reference cannot.
    /// </summary>
    [Obsolete("Use ProblemDetails.Detail instead.")]
    public string? UserMessage {
        get => ProblemDetails.Detail;
        set => ProblemDetails.Detail = value;
    }

    /// <summary>
    /// The problem details for this exception, can be used to communicate from server,
    /// through API, through SAL, to users.
    /// </summary>
    public ProblemDetails ProblemDetails { get; private set; } = new();

}
