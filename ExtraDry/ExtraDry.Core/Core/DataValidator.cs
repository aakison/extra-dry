namespace ExtraDry.Core;

/// <summary>
/// Designed to facility the validation of properties of objects, especially when not all of the object is validated.
/// </summary>
public class DataValidator {

    /// <summary>
    /// Validates all properties of the indicated object.
    /// </summary>
    /// <returns>True if validation successful.</returns>
    public bool ValidateObject(object target)
    {
        var validationContext = new ValidationContext(target, null, null);
        int previousCount = Errors.Count;
        Validator.TryValidateObject(target, validationContext, Errors, validateAllProperties: true);
        var validatableComplexProperties = target.GetType().GetProperties()
            .Where(e => e.PropertyType.GetInterface(nameof(IValidatableObject)) != null);
        foreach(var property in validatableComplexProperties) {
            var value = property.GetValue(target);
            if(value != null) {
                var innerContext = new ValidationContext(value, null, null);
                Validator.TryValidateObject(value, innerContext, Errors, validateAllProperties: true);
            }
        }
        int currentCount = Errors.Count;
        return previousCount == currentCount;
    }

    /// <summary>
    /// Validates the specified properties (by name) of the indicated object.
    /// </summary>
    /// <returns>True if validation successful.</returns>
    /// <example>
    /// `validator.ValidateProperties(obj, nameof(obj.Property1), nameof(obj.Property2));`
    /// </example>
    public bool ValidateProperties(object target, params string[] propertyNames)
    {
        return ValidateProperties(target, false, propertyNames);
    }

    /// <summary>
    /// Validates the specified properties (by name) of the indicated object.
    /// Ability provided to force checking for `[Required]` on string fields even if not attributed.
    /// </summary>
    /// <returns>True if validation successful.</returns>
    /// <example>
    /// `validator.ValidateProperties(obj, nameof(obj.Property1), nameof(obj.Property2));`
    /// </example>
    public bool ValidateProperties(object target, bool forceRequired, params string[] propertyNames)
    {
        var validationContext = new ValidationContext(target, null, null);
        int previousCount = Errors.Count;
        foreach(var propertyName in propertyNames) {
            validationContext.MemberName = propertyName;
            try {
                var value = target?.GetType()?.GetProperty(propertyName)?.GetValue(target);
                Validator.TryValidateProperty(value, validationContext, Errors);
                var missing = string.IsNullOrWhiteSpace(value as string);
                if(missing && forceRequired) {
                    Errors.Add(new ValidationResult($"{propertyName} is required."));
                }
            }
            catch(ArgumentException) {
                Errors.Add(new ValidationResult($"{propertyName} not on object."));
            }
        }
        int currentCount = Errors.Count;
        return previousCount == currentCount; ;
    }

    public void ThrowIfInvalid()
    {
        if(Errors.Any()) {
            var message = string.Join(", ", Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(message);
        }
    }

    /// <summary>
    /// A list of all errors found by this data validator for all validation calls.
    /// </summary>
    public IList<ValidationResult> Errors { get; } = new List<ValidationResult>();
}
