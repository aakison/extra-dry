using System.Xml.Serialization;

namespace ExtraDry.Core;

/// <summary>
/// Represents a set of rules that is applied to the <see cref="Field"/>.
/// The rules may be conditionally applied.
/// </summary>
public class RuleSet {

    /// <summary>
    /// The identifier of the <see cref="RuleSet"/>.
    /// </summary>
    [Required]
    [RegularExpression(ValidationExpression.IdentifierRegex, ErrorMessage = ValidationExpression.IdentifierMessage)]
    [StringLength(ValidationExpression.IdentifierMaxLength)]
    public string Identifier { get; set; }

    /// <summary>
    /// Defines the Identifier of the <see cref="TemplateCondition"/> to be applied to this <see cref="RuleSet"/>.
    /// </summary>
    public string Condition { get; set; }

    /// <summary>
    /// Default RuleSet is determined by the RuleSet not having a <see cref="Condition"/>.
    /// </summary>
    [XmlIgnore, JsonIgnore]
    public bool DefaultRule => string.IsNullOrEmpty(Condition);

    /// <summary>
    /// The rule that enforces all possible valid values for this <see cref="RuleSet"/>.
    /// </summary>
    [ValidateObject]
    public ValidValuesRule ValidValuesRule { get; set; }

    /// <summary>
    /// The rule that enforces default value for this <see cref="RuleSet"/>.
    /// </summary>
    [ValidateObject]
    public DefaultValueRule DefaultValueRule { get; set; }

    /// <summary>
    /// The rule that enforces if a value is required for this <see cref="RuleSet"/>.
    /// </summary>
    [ValidateObject]
    public RequiredRule RequiredRule { get; set; }
}
