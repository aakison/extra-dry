namespace ExtraDry.Blazor;

/// <summary>
/// Event args for Validation changes in input controls.
/// </summary>
public class ValidationEventArgs : EventArgs
{
    /// <summary>
    /// Indicates if the property is valid.
    /// </summary>
    public required bool IsValid { get; set; }

    /// <summary>
    /// The name of the property that is being validated.
    /// </summary>
    public required string MemberName { get; set; }

    /// <summary>
    /// In invalid, a user-consumable message for the validation error.
    /// </summary>
    public required string Message { get; set; }
}
