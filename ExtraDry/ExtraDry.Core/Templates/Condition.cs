namespace ExtraDry.Core;

/// <summary>
/// Represents a <see cref="Condition"/> of a <see cref="ExpandoSchema"/>.  This
/// defines the Identifier, ExecutionOrder and a collection of <see cref="ConditionTest"/>
/// which can be utilised in the <see cref="RuleSet"/> of a <see cref="ExpandoField"/>.
/// </summary>
public class Condition {

    /// <summary>
    /// The identifier of the <see cref="Condition"/>.
    /// </summary>
    [Required]
    public string Identifier { get; set; }

    /// <summary>
    /// Indicates the order that the condition should be executed in.  Execution should
    /// stop once a <see cref="Condition"/> has executed successfully.
    /// </summary>
    public int ExecutionOrder { get; set; }

    /// <summary>
    /// A collection of <see cref="ConditionTest"/> which will determine if this
    /// <see cref="Condition"/> has been met.  In the case when there is
    /// more than one <see cref="ConditionTest"/>, they are run on an "AND" condition.
    /// </summary>
    public List<ConditionTest> Tests { get; }
}
