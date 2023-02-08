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

    /// <summary>
    /// Create an exception with the indicated problem details
    /// </summary>
    public DryException(ProblemDetails? details) : base(details?.Title)
    {
        if(details != null) {
            ProblemDetails = details;
        }
    }

    /// <summary>
    /// Create an exception with information that will populate the inner ProblemDetails.
    /// </summary>
    public DryException(HttpStatusCode status, string message, string detail) : base(message)
    {
        ProblemDetails.Status = (int)status;
        ProblemDetails.Title = message;
        ProblemDetails.Detail = detail;
    }

    /// <summary>
    /// Create a simple exception with a message, prefer the constructor with (status, message,
    /// detail) over this one.
    /// </summary>
    public DryException(string message) : base(message) {
        ProblemDetails.Title = message;
    }

    /// <summary>
    /// Create a simple exception with a message and inner exception, prefer the constructor with
    /// (status, message, detail) over this one.
    /// </summary>
    public DryException(string message, Exception inner) : base(message, inner) {
        ProblemDetails.Title = message;
    }

    /// <summary>
    /// Create a simple exception with a message and detail, prefer the constructor with (status,
    /// message, detail) over this one.
    /// </summary>
    public DryException(string message, string detail) : base(message)
    {
        ProblemDetails.Title = message;
        ProblemDetails.Detail = detail;
    }

    /// <summary>
    /// The problem details for this exception, can be used to communicate from server,
    /// through API, through SAL, to users.
    /// </summary>
    public ProblemDetails ProblemDetails { get; private set; } = new();

}
