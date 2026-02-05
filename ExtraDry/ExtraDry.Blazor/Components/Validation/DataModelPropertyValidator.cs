using System.Linq.Expressions;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// A validator that validates a specific property on a data model.  Takes a model instance
/// and the name of the associated property to validate.  The "target" parameter of the Validate
/// is ignored and the Data Model is validated directly.
/// </summary>
public class DataModelPropertyValidator : IValidator
{
    public DataModelPropertyValidator(object model, string property)
    {
        Model = model;
        Property = property;
    }

    public DataModelPropertyValidator(object model, MemberExpression memberExpression)
    {
        if(memberExpression.Member is not PropertyInfo propertyInfo) {
            throw new ArgumentException("The member expression must point to a property.");
        }
        Model = model;
        Property = propertyInfo.Name;
    }

    public object Model { get; }

    public string Property { get; }

    public string Message { get; private set; } = "";

    public bool Validate(object? target)
    {
        var validator = new DataValidator();
        var result = validator.ValidateProperties(Model, Property);
        if(result == true) {
            Message = "";
        }
        else {
            Message = string.Join(" \r\n", validator.Errors.Select(e => e.ErrorMessage)) + " \r\n";
        }
        return result;
    }

    public IReadOnlyCollection<ValidationResult> Errors => [.. validator.Errors];

    private readonly DataValidator validator = new();
}
