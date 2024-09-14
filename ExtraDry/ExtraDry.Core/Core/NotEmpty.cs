namespace ExtraDry.Core;

/// <summary>
/// Ensures that a Guid or Decimal is not the empty or default value.  
/// Distinct from null in that it exists but does not have a value.  
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class NotEmptyAttribute : ValidationAttribute
{

    public NotEmptyAttribute() : base("The {0} field must not be empty.") { }

    public override bool IsValid(object? value)
    {
        if(value is null) {
            return true;
        }
        if(value is Guid guid) {
            return guid != Guid.Empty;
        }
        if(value is decimal d) {
            return d != 0;
        }
        return true;
    }
}
