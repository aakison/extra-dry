namespace ExtraDry.Blazor;

/// <summary>
/// Indicates that the specified method, typically on a ViewModel, 
/// is to be used to create a hyperlink for a particular property.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class HyperlinkAttribute(
    string propertyName) 
    : Attribute
{
    /// <summary>
    /// The name of a property on the target model which may be bound to this hyperlink.
    /// </summary>
    public string PropertyName { get; } = propertyName;
}
