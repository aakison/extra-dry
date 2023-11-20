namespace ExtraDry.Core;

/// <summary>
/// The count of discrete values for a given property.
/// </summary>
public class DataDistribution
{

    /// <summary>
    /// Create the set of data points for a single property name
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="counts"></param>
    public DataDistribution(string propertyName, Dictionary<string, int> counts)
    {
        Counts = counts;
        PropertyName = propertyName;
    }

    /// <summary>
    /// The name of the property that these counts were collected against.
    /// </summary>
    /// <example>trade</example>
    [JsonPropertyName("property")]
    public string PropertyName { get; protected set; }

    /// <summary>
    /// The list of discrete values and the associated count of their occurences.
    /// </summary>
    /// <example>{"electrical" : 1, "plumbing" : 2}</example>
    public Dictionary<string, int> Counts { get; } = new();

}
