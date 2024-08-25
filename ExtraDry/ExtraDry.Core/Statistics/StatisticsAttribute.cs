namespace ExtraDry.Core;

/// <summary>
/// Indicate that the property should be used to collect statistically information on the entity.
/// This is used in the rollup method PartialQueryable.ToStatisticsAsync to determine how to 
/// calculate the statistics for the entity.  Use sparingly to keep the cost of 
/// ToStatisticsAsync low.
/// </summary>
/// <remarks>
/// Apply statistics to a property.
/// </remarks>
/// <param name="type">The type of statistics to apply to this property.</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class StatisticsAttribute(Stats type) : Attribute
{

    /// <summary>
    /// The type of statistics collected for the indicated property.
    /// </summary>
    public Stats Stats { get; } = type;

}

