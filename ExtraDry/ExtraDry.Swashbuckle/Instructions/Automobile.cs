namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// A sample class used for demonstrating the common features of APIs.
/// </summary>
public class Automobile {

    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The make (manufacturer's name) of the car.
    /// </summary>
    /// <example>Toyota</example>
    [Filter(FilterType.Equals)]
    [Statistics(Stats.Distribution)]
    public string Make { get; set; } = string.Empty;

    /// <summary>
    /// The model name of the car.
    /// </summary>
    /// <example>FJ Cruiser</example>
    [Filter(FilterType.StartsWith)]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// The year that the car was first introduced.
    /// </summary>
    /// <example>2011</example>
    [Filter]
    [Statistics(Stats.Distribution)]
    public int Year { get; set; }

    /// <summary>
    /// A description of the intended regional market for the car.
    /// </summary>
    /// <example>Japan and Australasia</example>
    [Filter(FilterType.Contains)]
    [Statistics(Stats.Distribution)]
    public string Market { get; set; } = string.Empty;

    /// <summary>
    /// A description of the car, as sourced from Wikipedia.
    /// </summary>
    /// <example>Retro-styled body-on-frame mid-size SUV inspired by the Toyota FJ40.</example>
    [Filter(FilterType.Contains)]
    public string Description { get; set; } = string.Empty;

}

