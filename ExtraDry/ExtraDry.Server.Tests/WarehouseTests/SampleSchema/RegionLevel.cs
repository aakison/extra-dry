﻿namespace ExtraDry.Server.Tests.WarehouseTests;

/// <summary>
/// Represents the level of region in the ISO-3166 tree of geopolitical regions.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RegionLevel
{
    /// <summary>
    /// A global region, single root node for regions.
    /// </summary>
    Global = 0,

    /// <summary>
    /// The top-level of all regions.
    /// </summary>
    Country = 1,

    /// <summary>
    /// The first level divisions of countries, e.g. States and/or Territories
    /// </summary>
    Division = 2,

    /// <summary>
    /// Represents a division within a division, such as a City, Municipality, etc.
    /// </summary>
    Subdivision = 3,
}
