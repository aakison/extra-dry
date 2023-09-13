namespace ExtraDry.Core;

/// <summary>
/// Represents a <see cref="TemplateCondition"/> of a <see cref="Template"/>.  This
/// defines the Identifier, ExecutionOrder and a collection of <see cref="ConditionTest"/>
/// which can be utilised in the <see cref="RuleSet"/> of a <see cref="Field"/>.
/// </summary>
public class TemplateCondition {

    public TemplateCondition()
    {
        Tests = new ConditionTestCollection();
    }

    /// <summary>
    /// The identifier of the <see cref="TemplateCondition"/>.
    /// </summary>
    [Required]
    [RegularExpression(ValidationExpression.IdentifierRegex, ErrorMessage = ValidationExpression.IdentifierMessage)]
    [StringLength(ValidationExpression.IdentifierMaxLength)]
    public string Identifier { get; set; }

    /// <summary>
    /// Indicates the order that the condition should be executed in.  Execution should
    /// stop once a <see cref="TemplateCondition"/> has executed successfully.
    /// </summary>
    public int ExecutionOrder { get; set; }

    /// <summary>
    /// A collection of <see cref="ConditionTest"/> which will determine if this
    /// <see cref="TemplateCondition"/> has been met.  In the case when there is
    /// more than one <see cref="ConditionTest"/>, they are run on an "AND" condition.
    /// </summary>
    public ConditionTestCollection Tests { get; }
}
