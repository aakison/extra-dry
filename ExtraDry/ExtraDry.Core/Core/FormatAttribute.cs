namespace ExtraDry.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class FormatAttribute : Attribute
{
    /// <summary>
    /// The name of the icon for use with this entity.
    /// </summary>
    public string Icon { get; set; } = string.Empty;
}
