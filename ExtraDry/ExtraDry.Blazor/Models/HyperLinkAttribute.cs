namespace ExtraDry.Blazor;

/// <summary>
/// Indicates that the specified method, typically on a ViewModel, 
/// is to be used to create a hyperlink for a particular property.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class HyperLinkAttribute : Attribute
{
    /// <summary>
    /// The name of a property on the target model which may be bound to this hyperlink.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;
}
