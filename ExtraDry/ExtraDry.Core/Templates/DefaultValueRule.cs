namespace ExtraDry.Core;

/// <summary>
/// Defines the default value for the <see cref="Field"/>, to be populated
/// by the client when a <see cref="Form"/> is loaded. If the <see cref="Form"/>
/// is "cancelled", then the default should be reverted.
/// </summary>
/// <remarks>
/// This is a shallow wrapper to provide some future-proofing.
/// </remarks>
public class DefaultValueRule {

    [Required]
    public string Value { get; set; }
}
