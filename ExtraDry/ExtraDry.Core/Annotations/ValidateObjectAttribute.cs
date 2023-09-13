namespace ExtraDry.Core;

/// <summary>
/// Within the DataAnnotation framework, enables validates to validate on elements in a child collection.
/// Add `[ValidateObject]` attribute to make validation validate elements within that object.
/// </summary>
/// <remarks>
/// Adapted from a blog post http://www.technofattie.com/2011/10/05/recursive-validation-using-dataannotations.html
/// </remarks>
public class ValidateObjectAttribute : ValidationAttribute {

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if(value != null) {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null);

            Validator.TryValidateObject(value, context, results, true);

            if(results.Count != 0) {
                var compositeResults = new CompositeValidationResult(String.Format("Validation for {0} failed!", validationContext.DisplayName));
                results.AddRange(compositeResults.Results);

                return compositeResults;
            }
        }

        return ValidationResult.Success;
    }
}

public class CompositeValidationResult : ValidationResult {
    private readonly List<ValidationResult> _results = new List<ValidationResult>();

    public IEnumerable<ValidationResult> Results {
        get {
            return _results;
        }
    }

    public CompositeValidationResult(string errorMessage) : base(errorMessage)
    {
    }
}
