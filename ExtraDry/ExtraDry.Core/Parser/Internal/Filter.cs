namespace ExtraDry.Core.Parser.Internal;

public class Filter
{

    public static implicit operator Filter(string filter) {
        return FilterParser.Parse(filter);
    }

    public Filter(IEnumerable<FilterRule> rules)
    {
        foreach(var rule in rules) {
            var existing = Rules.FirstOrDefault(e => e.PropertyName == rule.PropertyName);
            if(existing == null) {
                Rules.Add(rule);
            }
            else {
                existing.Values.AddRange(rule.Values);
            }
        }
    }

    public FilterRule? TryGetRule(string propertyName)
    {
        return Rules.FirstOrDefault(e => e.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
    }

    public void RemoveRule(string propertyName)
    {
        var existing = TryGetRule(propertyName);
        if(existing != null) {
            Rules.Remove(existing);
        }
    }

    public List<FilterRule> Rules { get; } = [];
}
