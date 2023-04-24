namespace ExtraDry.Core;

/// <summary>
/// Actions that are used by the Rule Engine to determine how creates and updates should be processed.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleAction {

    /// <summary>
    /// Changes to values will always be allowed by the Rule Engine.
    /// </summary>
    Allow,

    /// <summary>
    /// Changes to values will always be ignored by the Rule Engine.
    /// </summary>
    Ignore,

    /// <summary>
    /// Values of `default` or `null` will be ignored, others will be allowed by the Rule Engine.
    /// </summary>
    IgnoreDefaults,

    /// <summary>
    /// Any attempt to change a value will raise an exception by the Rule Engine.
    /// Values of `default` or `null` do not trigger the exception.
    /// </summary>
    Block,

    /// <summary>
    /// Incoming values are not copied, instead the matching Entity Resolver (implements 
    /// `IEntityResolver`) is used to lookup the existing entity to link to.
    /// </summary>
    Link,

}
