namespace ExtraDry.Core;

/// <summary>
/// The result of a validation check.
/// </summary>
public enum ValidationStatus
{
    /// <summary>
    /// The validation rule has been checked and has passed.  Typically not used directly.
    /// </summary>
    Passed,

    /// <summary>
    /// The validation rule has not passed and will not pass but shouldn't alert the user overtly.
    /// For example, when a form is loaded and the field is invalid before a user has a chance to fill it in.
    /// </summary>
    Silent,

    /// <summary>
    /// The validation rule has not passed and the user should be alerted.
    /// </summary>
    Failed,
}
