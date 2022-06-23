namespace ExtraDry.Server.DataWarehouse.Builder;

/// <summary>
/// Represents the possible expansion of an Enum into a Dimension when Enums are annotated with [DimensionTable].
/// </summary>
public class EnumDimension {

    /// <summary>
    /// Not intended to be instantiated, used by fluent builder interface to build other objects.
    /// </summary>
    internal EnumDimension() { }

    /// <summary>
    /// The ID or value of the enum, used as the primary key in the dimension table.
    /// </summary>
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// The Name property from the Display attributes if they exist, or a title-cased version of the identifiers in code.
    /// </summary>
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The Description from the Display attributes.  If no descriptions, this will be ignored on build.
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// The ShortName from the Display attributes.  If no short names, this will be ignored on build.
    /// </summary>
    [StringLength(50)]
    public string? ShortName { get; set; }

    /// <summary>
    /// The GroupName from the Display attributes.  If no group names, this will be ignored on build.
    /// </summary>
    [StringLength(50)]
    public string? GroupName { get; set; }

    /// <summary>
    /// The Order from the Display attributes.  If no orders, this will be ignored on build.
    /// </summary>
    [Attribute]
    public int Order { get; set; }

}
