namespace ExtraDry.Core;

/// <summary>
/// Represents a Field in a User Defined Schema, this defines the name, data type, ordering etc.
/// </summary>
public class ExpandoField {

    /// <summary>
    /// A unique slug for the field that is auto-generated on create.
    /// </summary>
    /// <example>external-id</example>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Display Label
    /// </summary>
    public string Label { get; set; } = "label";

    /// <summary>
    /// A description for the field that can present more information than the name.
    /// This may be presented to users in the form of a tooltip or similar.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The data type for the field, these are high level data types like found in JavaScript.
    /// </summary>
    public ExpandoDataType DataType { get; set; }

    /// <summary>
    /// The default value for the field, to be populated by the client when a form is loaded.
    /// If the form is "cancelled", then the default should be reverted.
    /// </summary>
    public object? DefaultValue { get; set; }

    public string Icon { get; set; } = string.Empty;

    public string Placeholder { get; set; } = string.Empty;

    [Display(Name = "Required")]
    public bool IsRequired { get; set; }

    [Display(Name ="Max Text Length")]
    public int MaxLength { get; set; }

    [Display(Name = "Valid Values")]
    public List<string> ValidValues { get; set; } = new();

    public string RangeMinimum { get; set; } = string.Empty;

    public string RangeMaximum { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating how the Data Warehouse should interpret this value.
    /// </summary>
    public WarehouseBehavior WarehouseBehavior { get; set; }

    public int Order { get; set; }

    public ExpandoState State { get; set; } = ExpandoState.Draft;

    public IEnumerable<ValidationResult> ValidateValue(object? value)
    {
        var results = new List<ValidationResult>();

        if(IsRequired) {
            if(value == null) {
                results.Add(new ValidationResult($"{Label} is required.", new[] { Label }));
            }
        }

        var stringVal = value?.ToString();
        if(stringVal != null) {
            if(MaxLength > 0 && stringVal.Length > MaxLength) {
                results.Add(new ValidationResult($"{Label} exceeds Maxlength.", new[] { Label }));
            }

            if(ValidValues.Any() && !ValidValues.Contains(stringVal)) {
                results.Add(new ValidationResult($"{Label} does not exist in list of ValidValues.", new[] { Label }));
            }
        }

        if(int.TryParse(stringVal, out int intVal)) {
            if(int.TryParse(RangeMinimum, out int intRangeMin) && intVal < intRangeMin) {
                results.Add(new ValidationResult($"{Label} does not meet RangeMinimum set.", new[] { Label }));
            }

            if(int.TryParse(RangeMaximum, out int intRangeMax) && intVal > intRangeMax) {
                results.Add(new ValidationResult($"{Label} exceeds RangeMaximum set.", new[] { Label }));
            }

            var intValidValues = ValidValues.ConvertAll(s => int.TryParse(s, out int x) ? x : 0);
            if(ValidValues.Any() && !intValidValues.Contains(intVal)) {
                results.Add(new ValidationResult($"{Label} does not exist in list of ValidValues.", new[] { Label }));
            }
        }

        if(DateTime.TryParse(stringVal, out DateTime dateTimeVal)) {
            if(DateTime.TryParse(RangeMinimum, out DateTime dtRangeMin) && dateTimeVal < dtRangeMin) {
                results.Add(new ValidationResult($"{Label} does not meet RangeMinimum set.", new[] { Label }));
            }

            if(DateTime.TryParse(RangeMaximum, out DateTime dtRangeMax) && dateTimeVal > dtRangeMax) {
                results.Add(new ValidationResult($"{Label} exceeds RangeMaximum set.", new[] { Label }));
            }
        }

        return results;
    }
}

