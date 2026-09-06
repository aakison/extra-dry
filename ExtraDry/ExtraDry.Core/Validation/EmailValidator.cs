namespace ExtraDry.Core.Validation;

public class EmailValidator : RegexValidator
{
    public EmailValidator()
    {
        Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        ErrorMessage = "Please enter a valid email address.";
    }
}
