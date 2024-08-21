namespace ExtraDry.Core;

/// <summary>
/// Represents the structure of user defined fields that can be added to a Subject.
/// </summary>
public class ExpandoSchema : IValidatableObject
{

    /// <summary>
    /// The target type of the Subject that the <see cref="ExpandoSchema"/> provides custom fields for.
    /// </summary>
    [Required]
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    public string TargetType { get; set; } = string.Empty;

    /// <summary>
    /// A collection of fields contained in the schema.
    /// </summary>
    public List<ExpandoField> Fields { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var slugs = Fields.Select(e => e.Slug);
        if(slugs.Count() != slugs.Distinct().Count()) {
            yield return new ValidationResult("Duplicate Slugs found.", [nameof(ExpandoField.Slug)]);
        }
    }

    public IEnumerable<ValidationResult> ValidateValues(ExpandoValues values)
    {
        var results = new List<ValidationResult>();

        foreach(var field in Fields) {
            values.TryGetValue(field.Slug, out var fieldValue);
            results.AddRange(field.ValidateValue(fieldValue));
        }

        return results;
    }
}
