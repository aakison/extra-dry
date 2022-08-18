#nullable enable

namespace ExtraDry.Blazor;

public class ValueDescription : ISubjectViewModel {

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

    public string Subtitle { get; set; } = string.Empty;

    public string Thumbnail => string.Empty;

    /// <summary>
    /// Indicates if the value should be in generated structures such as dropdown filter lists.
    /// </summary>
    public bool AutoGenerate { get; set; }

    public string Code => string.Empty;

    public string Caption => Title;

    public string Description => string.Empty;

    public override bool Equals(object? obj) => (obj as ValueDescription)?.Key == Key;

    public override int GetHashCode() => Key.GetHashCode();

}
