namespace ExtraDry.Core;

/// <summary>
/// Represents an exception for an argument that is provided twice and doesn't match. In 
/// particular, use when controller gets both URI and body versions of an ID.
/// </summary>
[Serializable]
public sealed class ArgumentMismatchException : ArgumentException {

    /// <inheritdoc cref="ArgumentMismatchException" />
    public ArgumentMismatchException(string message, string paramName) : base(message, paramName) {
        UserMessage = string.Format(UserMessage, paramName);
    }

    /// <inheritdoc cref="ArgumentMismatchException" />
    public ArgumentMismatchException(string message, Exception inner) : base(message, inner) { 
    
    }

    /// <inheritdoc cref="ArgumentMismatchException" />
    public ArgumentMismatchException(string message, string paramName, string userMessage) : base(message, paramName)
    {
        UserMessage = userMessage;
    }

    /// <summary>
    /// If available, an exception message that is suitable to show to users.  E.g. certain 
    /// validation exceptions can be shown, but null reference cannot.
    /// </summary>
    public string UserMessage { get; set; } = @"When the {0} of an entity occurs in both the URI and in the body of the of the request, they must be identical.  This happens particularly during an update (POST).";
}
