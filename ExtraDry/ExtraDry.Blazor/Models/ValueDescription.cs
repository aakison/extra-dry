#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ExtraDry.Blazor;

public class ValueDescription : ITitleSubject, IPreviewSubject {

    public ValueDescription(object key, MemberInfo memberInfo)
    {
        Key = key;

        var display = memberInfo.GetCustomAttribute<DisplayAttribute>();
        Title = display?.Name 
            ?? memberInfo.Name; 
        // TODO: Format display name with global title case converter.
        AutoGenerate = display?.GetAutoGenerateField() ?? true;
    }

    public object Key { get; set; }

    public string Title { get; set; }

    public string? Subtitle { get; set; } = string.Empty;

    public string? CssClass { get; set; } = string.Empty;

    public string? Thumbnail { get; set; }

    /// <summary>
    /// Indicates if the value should be in generated structures such as dropdown filter lists.
    /// </summary>
    public bool AutoGenerate { get; set; }
}
