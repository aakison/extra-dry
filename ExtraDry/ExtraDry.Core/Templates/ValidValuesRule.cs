namespace ExtraDry.Core;

/// <summary>
/// A maximum length applies to a text field and determines the maximum number of characters allowed for the text.
/// When parsing an IFC file, this rule is set to the length of the largest field value.
/// Note that values may exist in the database or on the server that are not in this valid value rule,
/// these should be treated as deprecated values, they may be retained by the UI for this value but may not
/// be entered as new values.
/// </summary>
public class ValidValuesRule {

    /// <summary>
    /// Create a new rule that applies validation to a set of strings, typically used to make a string value a pick-list.
    /// </summary>
    public ValidValuesRule()
    {
        Initialize();
    }

    private void Initialize()
    {
        ValidValues = new ValidValueCollection();
    }

    /// <summary>
    /// The collection of values that are acceptable
    /// </summary>
    public ValidValueCollection ValidValues { get; private set; }
}
