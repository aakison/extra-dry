namespace ExtraDry.Core;

/// <summary>
/// Represents the structure of custom fields that can be added to a Subject.
/// </summary>
public class ExpandoSchema : IValidatableObject {

    /// <summary>
    /// The target type of the class that the <see cref="ExpandoSchema"/> provides custom fields for.
    /// </summary>
    [Required]
    public string TargetType { get; set; } = string.Empty;

    public List<ExpandoSection> Sections { get; set;} = new();

    public ExpandoState State { get; set; } = ExpandoState.Active;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var listIds = Sections.SelectMany(s => s.Fields).Select(e => e.Slug);
        if(listIds.Count() != listIds.Distinct().Count()) {
            yield return new ValidationResult("Duplicate Id's found.", new[] { nameof(ExpandoField.Slug) });
        }
    }
}
