namespace ExtraDry.Blazor.Components;

public class RegexValidator : IValidator
{
    public required string Pattern { get; init; }

    public required string ErrorMessage { get; init; }

    public bool Validate(object? target)
    {
        Results.Clear();
        if(target is not string str || !System.Text.RegularExpressions.Regex.IsMatch(str, Pattern)) {
            Results.Add(new ValidationResult(ErrorMessage));
            return false;
        }
        return true;
    }
    public string Message => Errors.Count == 0 ? ""
        : "  * " + string.Join("\r\n  * ", Errors.Select(e => e.ErrorMessage)) + "\r\n";

    public IReadOnlyCollection<ValidationResult> Errors => [.. Results];

    private List<ValidationResult> Results { get; } = [];
}
