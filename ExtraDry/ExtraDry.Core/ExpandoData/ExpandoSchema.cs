namespace ExtraDry.Core;

/// <summary>
/// Represents the structure of user defined fields that can be added to a Subject.
/// </summary>
public class ExpandoSchema : IValidatableObject {

    /// <summary>
    /// The target type of the class that the <see cref="ExpandoSchema"/> provides custom fields for.
    /// </summary>
    [Required]
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    public string TargetType { get; set; } = string.Empty;

    public List<ExpandoSection> Sections { get; set;} = new();

    public ExpandoState State { get; set; } = ExpandoState.Active;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var listIds = Sections.SelectMany(s => s.Fields).Select(e => e.Slug);
        if(listIds.Count() != listIds.Distinct().Count()) {
            yield return new ValidationResult("Duplicate Slugs found.", new[] { nameof(ExpandoField.Slug) });
        }
    }

    public IEnumerable<ValidationResult> ValidateValues(ExpandoValues values)
    {
        var results = new List<ValidationResult>();
        var fields = Sections.SelectMany(e => e.Fields);

        foreach(var schemaField in fields) {
            values.Values.TryGetValue(schemaField.Slug, out var fieldValue);
            results.AddRange(schemaField.ValidateValue(fieldValue));
        }

        return results;
    }
}
