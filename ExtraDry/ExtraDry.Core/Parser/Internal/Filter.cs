namespace ExtraDry.Core.Parser.Internal;

public class Filter
{
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

    public List<FilterRule> Rules { get; } = [];
}
