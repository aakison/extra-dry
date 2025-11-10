namespace ExtraDry.Core;

/// <summary>
/// The result of a validation attempt.
/// </summary>
public class ValidationInfo
{
    public ValidationInfo(string memberName, ValidationStatus status, string message)
    {
        MemberName = memberName;
        Status = status;
        Message = message;
    }

    /// <summary>
    /// The name of the member, typically a property, that the validation information is about.
    /// </summary>
    public string MemberName { get; init; } = "";

    /// <summary>
    /// The result of this validation info.
    /// </summary>
    public ValidationStatus Status { get; set; }

    /// <summary>
    /// On any failure, the message that should be displayed to the user.
    /// </summary>
    public string Message { get; set; } = "";
}
