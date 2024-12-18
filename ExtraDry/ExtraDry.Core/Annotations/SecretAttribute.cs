namespace ExtraDry.Core;

/// <summary>
/// Indicates that the property is a secret and should not be displayed in logs or other output.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class SecretAttribute : Attribute
{

}
