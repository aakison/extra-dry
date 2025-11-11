namespace ExtraDry.Core;

/// <summary>
/// The result of a validation attempt.
/// </summary>
public class ValidationInfo(string memberName, ValidationStatus status, string message)
{

    /// <summary>
    /// The name of the member, typically a property, that the validation information is about.
    /// </summary>
    public string MemberName { get; init; } = memberName;

    /// <summary>
    /// The result of this validation info.
    /// </summary>
    public ValidationStatus Status { get; set; } = status;

    /// <summary>
    /// On any failure, the message that should be displayed to the user.
    /// </summary>
    public string Message { get; set; } = message;
}
