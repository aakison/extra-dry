using System.Text.Json.Serialization;

namespace ExtraDry.Core;

/// <summary>
/// Actions that are used by rules to determine how creates and updates should be processed.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleAction {

    /// <summary>
    /// Changes to values will always be allowed by `RuleEngine`.
    /// </summary>
    Allow,

    /// <summary>
    /// Changes to values will always be ignored by `RuleEngine`.
    /// </summary>
    Ignore,

    /// <summary>
    /// Values of `default` or `null` will be ignored, others will be allowed by `RuleEngine`.
    /// </summary>
    IgnoreDefaults,

    /// <summary>
    /// Any attempt to change a value will raise an exception by the `RuleEngine`.
    /// Values of `default` or `null` do not trigger the exception.
    /// </summary>
    Block,

    /// <summary>
    /// Incoming values are not copied, instead the matching `IEntityResolver` is
    /// used to lookup the existing entity to link to.
    /// </summary>
    Link,

}
