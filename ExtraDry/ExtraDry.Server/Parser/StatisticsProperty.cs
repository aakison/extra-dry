using System.Reflection;

namespace ExtraDry.Server.Internal;

/// <summary>
/// Represents information about a property on an Entity that is decorated with StatisticsAttribute
/// in the ModelDescription.
/// </summary>
internal class StatisticsProperty {

    public StatisticsProperty(PropertyInfo property, Stats stats)
    {
        Property = property;
        Stats = stats;
    }

    public PropertyInfo Property { get; }

    public Stats Stats { get; }
}
