namespace Sample.Shared;

/// <summary>
/// Represents the level of region in the ISO-3166 tree of geopolitical regions.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RegionLevel {

    /// <summary>
    /// The top-level of all regions.
    /// </summary>
    Country = 0,

    /// <summary>
    /// The first level divisions of countries, e.g. States and/or Territories
    /// </summary>
    Division = 1,

    /// <summary>
    /// Represents a division within a division, such as a City, Municipality, etc.
    /// </summary>
    Subdivision = 2,

}
