namespace ExtraDry.Core;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MarkdownAttribute : ValidationAttribute
{
    public MarkdownAttribute(MarkdownSupportType supportType)
    {
        SupportType = supportType;
    }

    public MarkdownSupportType SupportType { get; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Validation logic implemented in P1-server-side-validation
        return ValidationResult.Success;
    }
}
