namespace ExtraDry.Core;

/// <summary>
/// Represents a single control for the display or data entry of a field.
/// </summary>
public class FormControl {

    /// <summary>
    /// The type of the control that is being displayed.
    /// This may affect the actual control as well as layout options and label alignment options.
    /// </summary>
    [Required]
    [EnumDataType(typeof(FormControlType), ErrorMessage = ValidationExpression.FormControlTypeMessage)]
    public FormControlType ControlType { get; set; }

    /// <summary>
    /// The label to show associated with this control.
    /// Do not supply punctuation with this label as the form layout engine will provide the appropriate punctuation (such as a trailing colon).
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// The text to display in a textbox when it is empty.
    /// This may allow the renderer to ignore the label or not at its discretion.
    /// </summary>
    public string Placeholder { get; set; }

    /// <summary>
    /// The sample data to display during form design.
    /// If this field is specified, it may use this instead of the placeholder at the render's discretion.
    /// </summary>
    public string SampleData { get; set; }

    /// <summary>
    /// The name of the template that the field is for.
    /// If this is for the currently active template, then this can be left blank and the FieldName is sufficient.
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// The name of the field that this control renders.
    /// If the field is for a different template, then specify the template's name in the TemplateName property.
    /// </summary>
    public string FieldName { get; set; }

    /// <summary>
    /// An optional regular expression that will automatically populate the control based on the contents of the regex.
    /// This regular expression requires a grouping mechanism (i.e. parentheses) to indicate the body to match.
    /// For example, an AutoMatch of "23(\d\d)67" run against "123456789" will return "45".
    /// However, an AutoMatch of "1\d+" will not return anything as it doesn't have a group.
    /// Note that this will work with both FormControlType.Input and FormControlType.Display.
    /// This behaviour allows AutoMatch to be the exclusive mechanism for inputting data into a field.
    /// If multiple matches are found, the last match will be used.
    /// </summary>
    public string AutoMatch { get; set; }

    /// <summary>
    /// A property that maps to the Field that is referenced by the FieldName.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the FieldName cannot be found, this will throw an InvalidOperationException.</exception>
    [JsonIgnore]
    public Field Field {
        get {
            if(FieldName == null) {
                return null;
            }
            if(String.IsNullOrEmpty(TemplateName)) {
                return GetFieldFromLocalTemplate(FieldName);
            }
            else {
                return GetFieldFromNamedTemplate(TemplateName, FieldName);
            }
        }
        set {
            if(value == null) {
                TemplateName = null;
                FieldName = null;
            }
            else {
                TemplateName = value.Parent.Name;
                FieldName = value.Name;
            }
        }
    }

    private Field GetFieldFromNamedTemplate(string templateName, string fieldName)
    {
        try {
            var config = ParentGroup.AncestorForm.ParentTemplate.FetchConfiguration();
            var template = config.Templates.FirstOrDefault(e => e.Name.ToLower() == templateName.ToLower());
            if(template == null) {
                return null;
            }
            var field = template.Fields.FirstOrDefault(e => e.Name.ToLower() == fieldName.ToLower());
            if(field == null) {
                return null;
            }
            return field;
        }
        catch(NullReferenceException) {
            return null;
        }
    }

    private Field GetFieldFromLocalTemplate(string fieldName)
    {
        var template = ParentGroup.AncestorForm.ParentTemplate;
        return template.Fields.FirstOrDefault(e => e.Name.ToLower() == fieldName.ToLower());
    }

    /// <summary>
    /// The parent Group for this control.
    /// </summary>
    [JsonIgnore]
    public FormGroup ParentGroup { get; internal set; }
}
