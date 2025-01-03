namespace Sample.Shared;

/// <summary>
/// Represents a soft-delete capable state for Sectors which indicate what services companies can
/// provide.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
[DimensionTable("Sector Status")]
public enum SectorState
{
    /// <summary>
    /// The sector is currently active and can be assigned to new companies.
    /// </summary>
    Active = 0,

    /// <summary>
    /// The sector is inactive and no longer in use, but still might be attached to historical
    /// companies.
    /// </summary>
    Inactive = 1,
}
