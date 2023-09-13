namespace ExtraDry.Core;

/// <summary>
/// List of operators that can be used in a <see cref="ConditionTest"/>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ConditionTestOperators
{
    EqualTo,
    NotEqualTo,
    LessThan,
    GreaterThan,
    Contains
}
