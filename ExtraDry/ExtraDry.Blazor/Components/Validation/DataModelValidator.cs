namespace ExtraDry.Blazor.Components;

/// <summary>
/// Validator that validates an entire data model instance using data annotations.
/// </summary>
public class DataModelValidator(
    object model) 
    : IValidator
{

    /// <summary>
    /// The model instance to validate.
    /// </summary>
    public object Model { get; } = model;

    /// <inheritdoc />
    public bool Validate(object? target)
    {
        var validator = new DataValidator();
        return validator.ValidateObject(Model);
    }

    /// <inheritdoc />
    public string Message => Errors.Count == 0 ? ""
        : "  * " + string.Join("\r\n  * ", Errors.Select(e => e.ErrorMessage)) + "\r\n";

    /// <inheritdoc />
    public IReadOnlyCollection<ValidationResult> Errors => [.. validator.Errors];

    private readonly DataValidator validator = new();
}
