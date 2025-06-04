namespace ExtraDry.Core;

/// <summary>
/// Provides a type of a <see cref="IListService{T}"/> that can be used to populate a list of options for a property in a form.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ListServiceAttribute(Type providerType) : Attribute
{
    /// <summary>
    /// Gets the type of the provider that will be used to populate the list of options.
    /// </summary>
    public Type ProviderType { get; } = providerType;

    /// <summary>
    /// Indicates whether the property is required. If true, the property must have a value when the form is submitted.
    /// If false the string entered by the user will be used.
    /// </summary>
    public bool Required { get; set; } = true; 

}
