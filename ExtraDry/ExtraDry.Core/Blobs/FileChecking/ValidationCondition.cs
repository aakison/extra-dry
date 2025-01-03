namespace ExtraDry.Core;

/// <summary>
/// Indicates the condition that a validation rule is applied. This is typically used to limit
/// client-side validation for performance reasons.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ValidationCondition
{
    /// <summary>
    /// Perform validation the server, but not on the client.
    /// </summary>
    ServerSide,

    /// <summary>
    /// Never perform validation. This is not recommended.
    /// </summary>
    Never,

    /// <summary>
    /// Always perform validation. This can be used to force validation on the client.
    /// </summary>
    Always,
}
