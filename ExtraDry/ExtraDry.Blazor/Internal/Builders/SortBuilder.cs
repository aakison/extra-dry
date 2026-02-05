namespace ExtraDry.Blazor;

public class SortBuilder
{
    public string SortProperty { get; set; } = string.Empty;

    public bool Ascending { get; set; } = true;

    public string Build()
    {
        return (Ascending, SortProperty) switch {
            (_, "") => "",
            (true, _) => $"+{SortProperty}",
            (false, _) => $"-{SortProperty}",
        };
    }
}
