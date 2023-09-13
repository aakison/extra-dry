using System.Xml.Serialization;

namespace ExtraDry.Core;

/// <summary>
/// Represents a Field of a Template, this defines the name, data type, etc. for Values that are assigned Tags.
/// </summary>
public class Field {
    private string defaultValueFromRuleSet = string.Empty;
    private string defaultValue = string.Empty;
    private ValidValuesRule validValuesRuleFromRuleSet;
    private ValidValuesRule validValuesRule;

    /// <summary>
    /// Creates a new Field definition.
    /// </summary>
    public Field()
    {
        RuleSets = new RuleSetCollection {
            OnInsert = UpdateValidValuesRuleAndDefaultValue
        };
    }

    /// <summary>
    /// The identifier of the <see cref="Field"/>.
    /// </summary>
    
    [Required]
    [RegularExpression(ValidationExpression.IdentifierRegex, ErrorMessage = ValidationExpression.IdentifierMessage)]
    [StringLength(ValidationExpression.IdentifierMaxLength)]
    public string Identifier { get; set; }

    /// <summary>
    /// [Obsolete] The name of the Field, this is shown to users and can contain any valid printable characters.
    ///
    /// Removed the Obsolete attribute since the XMLSerializer ignores any attributes marked as Obsolete - this is a breaking change for Capture
    /// </summary>
    //[Obsolete("Use DisplayName.")]
    
    public string Name { get; set; }

    /// <summary>
    /// The name of the Field, this will be displayed to the user and can be any printable string.
    /// </summary>
    
    public string DisplayName { get; set; }

    /// <summary>
    /// The data type for the field, these are high level data types like found in JavaScript.
    /// </summary>
    
    [EnumDataType(typeof(DataType), ErrorMessage = ValidationExpression.DataTypeMessage)]
    public DataType DataType { get; set; }

    /// <summary>
    /// [Obsolete] The default value for the field, to be populated by the client when a form is loaded.
    /// If the form is "cancelled", then the default should be reverted.
    /// </summary>
    /// <remarks>
    /// This property either be set directly or when <see cref="RuleSets"/> are added to the <see cref="Field"/>.
    /// If this property is set directly and via <see cref="RuleSets"/> then <see cref="RuleSets"/> takes priority.
    ///
    /// Removed the Obsolete attribute since the XMLSerializer ignores any attributes marked as Obsolete - this is a breaking change for Capture
    /// </remarks>
    //[Obsolete("Use the default rule in Rules to get this object.")]
    
    public string Default {
        get {
            return string.IsNullOrEmpty(defaultValueFromRuleSet) ? defaultValue : defaultValueFromRuleSet;
        }
        set {
            defaultValue = value;
        }
    }

    // (it will not indicated any type of warning image or message).
    
    public string DefaultIcon { get; set; }

    /// <summary>
    /// [Obsolete] The rule that enforces all possible valid values for this field.
    /// This is null if there is no rule related to valid values.
    /// </summary>
    /// <remarks>
    /// This property either be set directly or when <see cref="RuleSets"/> are added to the <see cref="Field"/>.
    /// If this property is set directly and via <see cref="RuleSets"/> then <see cref="RuleSets"/> takes priority.
    ///
    /// Removed the Obsolete attribute since the XMLSerializer ignores any attributes marked as Obsolete - this is a breaking change for Capture
    /// </remarks>
    //[Obsolete("Use the default rule in RuleSets to get this object.")]
    public ValidValuesRule ValidValuesRule {
        get {
            return validValuesRuleFromRuleSet != null ? validValuesRuleFromRuleSet : validValuesRule;
        }
        set {
            validValuesRule = value;
        }
    }

    /// <summary>
    /// [Obsolete] A description for the field that can present more information than the name.
    /// This may be presented to users in the form of a tooltip or similar.
    ///
    /// Removed the Obsolete attribute since the XMLSerializer ignores any attributes marked as Obsolete - this is a breaking change for Capture
    /// </summary>
    //[Obsolete("If this was being used for a tooltip then migrate to the Tooltip property.")]
    public string Description { get; set; }

    /// <summary>
    /// A tooltip that provides additional information about the field.
    /// Do not assume that this is shown to the user as it will likely be hidden by default.
    /// </summary>
    
    public string Tooltip { get; set; }

    /// <summary>
    /// The parent template that this field is attached to.
    /// </summary>
    [XmlIgnore, JsonIgnore]
    public Template Parent { get; internal set; }

    /// <summary>
    /// Gets or sets a value indicating how the Data Warehouse should interpret this value.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WarehouseBehaviour WarehouseBehaviour { get; set; }

    /// <summary>
    /// Used to determine if the field is part the system or has been custom built.
    /// </summary>
    public bool BuiltIn { get; set; }

    /// <summary>
    /// A collection of <see cref="RuleSet"/> which will be applied to this <see cref="Field"/>.
    /// </summary>
    public RuleSetCollection RuleSets { get; }

    private void UpdateValidValuesRuleAndDefaultValue(RuleSet rule)
    {
        if(rule.DefaultRule) {
            if(validValuesRuleFromRuleSet == null) {
                validValuesRuleFromRuleSet = rule.ValidValuesRule;
            }

            if(rule.DefaultValueRule != null && string.IsNullOrEmpty(defaultValueFromRuleSet)) {
                defaultValueFromRuleSet = rule.DefaultValueRule.Value;
            }
        }
    }
}
