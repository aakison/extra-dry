namespace ExtraDry.Core;

/// <summary>
/// Static expressions and messages for validation of model classes.
/// </summary>
public static class ValidationExpression {

    /// <summary>
    /// A standard identifier regex for model classes.
    /// </summary>
    public const string IdentifierRegex = @"[a-zA-Z0-9]*";

    /// <summary>
    /// Message for incorrect identifier.
    /// </summary>
    public const string IdentifierMessage = "Identifier must contain only letters and digits.";

    /// <summary>
    /// Maximum length for identifier.
    /// </summary>
    public const int IdentifierMaxLength = 25;

    /// <summary>
    /// A standard email regex for model classes.  Allows some false positives without becomming too large.
    /// </summary>
    public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

    /// <summary>
    /// Message for incorrect e-mail.
    /// </summary>
    public const string EmailMessage = "Email address must have a valid format.";

    /// <summary>
    /// A standard phone number regex for model classes.  Fairly flexible for international numbers and formats.
    /// </summary>
    public const string PhoneNumberRegex = @"^\+?[0-9\(\)\-\.\s]{0,25}$";

    /// <summary>
    /// Message for incorrect phone number.
    /// </summary>
    public const string PhoneNumberRegexMessage = "Phone number must only contain digits and the characters '+', '-', '(', and ')'.";

    /// <summary>
    /// A standard client short name regex for model classes.  This is more constrained than the typical identifier.
    /// </summary>
    public const string ClientShortNameRegex = @"[a-z][a-z0-9]*";

    /// <summary>
    /// Message for incorrect client short name.
    /// </summary>
    public const string ClientShortNameMessage = "Client Short Name must start with a letter and contain only letters and digits.";

    /// <summary>
    /// A standard role name regex for model classes.
    /// </summary>
    public const string RoleRegex = @"[a-zA-Z]\w{1,23}"; // Start with a letter.

    /// <summary>
    /// Message for incorrect role name.
    /// </summary>
    public const string RoleMessage = "Role names must contain only letters and be between 2 and 20 characters long.";

    /// <summary>
    /// Message for incorrect data type.
    /// </summary>
    public const string DataTypeMessage = "Data type value is not supported.";

    /// <summary>
    /// Message for incorrect <see cref="Template.GroupContentType"/>
    /// </summary>
    public const string GroupContentTypeMessage = "Group Content Type value is not supported.";

    /// <summary>
    /// Message for incorrect <see cref="Template.FormControlType"/>
    /// </summary>
    public const string FormControlTypeMessage = "Form Control Type value is not supported.";

    /// <summary>
    /// Message for incorrect percent range
    /// </summary>
    public const string PercentMessage = "{0} percent must be between {1} and {2}";

    /// <summary>
    /// Message for incorrect range
    /// </summary>
    public const string RangeMessage = "{0} must be between {1} and {2}";
}
