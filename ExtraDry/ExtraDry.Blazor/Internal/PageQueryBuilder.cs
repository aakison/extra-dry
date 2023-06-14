namespace ExtraDry.Blazor.Internal;

public class PageQueryBuilder {

    public PageQueryBuilder() { 
        TextFilter = new TextFilterBuilder() { FilterName = "Keywords" };
        Filters.Add(TextFilter);
    }

    public event EventHandler? OnChanged;

    public void NotifyChanged()
    {
        Query = Build();
        OnChanged?.Invoke(this, EventArgs.Empty);
    }

    public PageQuery Build()
    {
        Query = new PageQuery() {
            Filter = string.Join(' ', Filters.Select(e => e.Build()).Where(e => !string.IsNullOrWhiteSpace(e))).Trim(),
        };
        return Query;
    }

    public List<FilterBuilder> Filters { get; } = new();

    public TextFilterBuilder TextFilter { get; }

    public PageQuery Query { get; private set; } = new();

}

public abstract class FilterBuilder
{
    public string FilterName { get; set; } = string.Empty;

    public abstract string Build();
}

public class TextFilterBuilder : FilterBuilder {
    public string Keywords { get; set; } = string.Empty;

    public override string Build() => Keywords.Trim();
}

public class EnumFilterBuilder : FilterBuilder
{
    public List<string> Values { get; } = new();

    public override string Build() => Values.Any() ? $"{FilterName}:{QuotedValues}" : "";

    private string QuotedValues => string.Join('|', Values.Where(e => !string.IsNullOrWhiteSpace(e)).Select(QuotedValue));

    private string QuotedValue(string value) => value.Contains(' ') || value.Contains('|') ? $"\"{value}\"" : value;
}
