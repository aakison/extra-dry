namespace ExtraDry.Server.Internal;

internal class FilterRule
{

    public FilterRule(string propertyName, FilterRule values)
    {
        PropertyName = propertyName;
        LowerBound = values.LowerBound;
        UpperBound = values.UpperBound;
        Values.AddRange(values.Values);
    }

    public FilterRule(string propertyName, BoundRule lowerBound, string lowerValue, BoundRule upperBound, string upperValue)
    {
        PropertyName = propertyName;
        LowerBound = lowerBound;
        UpperBound = upperBound;
        Values.Add(lowerValue);
        Values.Add(upperValue);
    }

    public FilterRule(string propertyName, IEnumerable<string> values)
    {
        PropertyName = propertyName;
        Values = new List<string>(values);
    }

    public FilterRule(string propertyName, string value)
    {
        PropertyName = propertyName;
        Values = [value];
    }

    public BoundRule LowerBound { get; } = BoundRule.None;

    public BoundRule UpperBound { get; } = BoundRule.None;

    public string PropertyName { get; private set; }

    public List<string> Values { get; } = [];
}
