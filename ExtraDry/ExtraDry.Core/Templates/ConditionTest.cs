namespace ExtraDry.Core;

/// <summary>
/// Represents a <see cref="ConditionTest"/> of a <see cref="Condition"/>.  This defines the <see cref="Templates.Field"/> (via it's Identifier)
/// to compare, the <see cref="Operator"/> to be applied and the <see cref="Value"/> to compare.
/// </summary>
public class ConditionTest {

    /// <summary>
    /// The Identifier of the <see cref="Templates.Field"/> to compare.
    /// </summary>
    [Required]
    public Guid FieldUuid { get; set; }

    /// <summary>
    /// The operator to be applied.
    /// </summary>
    public ConditionTestOperators Operator { get; set; }

    /// <summary>
    /// The value to compare.
    /// </summary>
    [Required]
    public string Value { get; set; }

}
