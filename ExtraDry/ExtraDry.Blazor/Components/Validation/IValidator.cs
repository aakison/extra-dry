namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents a validator that can validate an object or a property on an object.
/// Acts as a interface for validation done in the XxxField and DryXxxField components.
/// </summary>
public interface IValidator
{
    /// <summary>
    /// Validates the target.  This may be the indicated target or the target may
    /// be defined in the validator itself (e.g. DataModelPropertyValidator).
    /// </summary>
    bool Validate(object? target);

    /// <summary>
    /// If the last validation failed, this contains the error messages.  Otherwise empty.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// If the last validation failed, this contains the validation results.  Otherwise empty.
    /// </summary>
    IReadOnlyCollection<ValidationResult> Errors { get; }

}
