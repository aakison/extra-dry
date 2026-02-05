namespace ExtraDry.Blazor.Components;

public partial class OptionField<TValue> : FieldBase<TValue>
{
    public OptionField()
    {
        if(typeof(TValue).IsEnum) {
            KeyFunc = EnumKeyFunc;
        }
        else {
            KeyFunc = IdentifierKeyFunc;
        }
        if(typeof(TValue).IsAssignableTo(typeof(IResourceIdentifiers))) {
            TitleFunc = ResourceTitleFunc;
        }
        else if(typeof(TValue).IsEnum) {
            TitleFunc = EnumTitleFunc;
        }
        else {
            TitleFunc = ObjectTitleFunc;
        }
    }

    /// <summary>
    /// Set of values to select from, any object can be used and the display text is either
    /// IResourceIdentifiers.Title or object.ToString() value.
    /// </summary>
    [Parameter, EditorRequired]
    public IList<TValue> Options { get; set; } = null!;

    /// <summary>
    /// A function that maps an option value to a display title for the user.  Default functions
    /// are provided for Enum, <see cref="IResourceIdentifiers"/>, or falls back to Object.ToString()
    /// </summary>
    [Parameter]
    public Func<TValue, string> TitleFunc { get; set; }

    /// <summary>
    /// A function that maps an option value to a key for the value (not shown to users put potentially
    /// useful for debugging).  Defaults map to known UUID, enum values, or falls back to random Guid.
    /// </summary>
    [Parameter]
    public Func<TValue, string> KeyFunc { get; set; }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "select", ReadOnlyCss, CssClass);

    private List<Option> InternalOptions { get; set; } = [];

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        InternalOptions = Options.Select(e => new Option { Value = e, Key = KeyFunc(e), Title = TitleFunc(e) }).ToList();
    }

    private const string EmptyDisplayText = "--empty--";

    private static string EnumTitleFunc(TValue value) => value is Enum enumValue ? DataConverter.DisplayEnum(enumValue) : EmptyDisplayText;

    private static string ResourceTitleFunc(TValue value) => (value as IResourceIdentifiers)?.Title ?? EmptyDisplayText;

    private static string ObjectTitleFunc(TValue value) => value?.ToString() ?? EmptyDisplayText;

    private static string EnumKeyFunc(TValue value) => $"{typeof(TValue).Name}-{value}";

    private static string IdentifierKeyFunc(TValue value) => ((value as IUniqueIdentifier)?.Uuid ?? new Guid()).ToString();

    private bool IsSelected(Option option) => (option.Value is null && Value is null) || (option.Value?.Equals(Value) ?? false);

    public class Option
    {
        public required string Key { get; init; }

        public required string Title { get; init; }

        public required TValue Value { get; init; }
    }

    private async Task NotifyInputByKey(ChangeEventArgs args)
    {
        var selected = InternalOptions.FirstOrDefault(e => e.Key == (string?)args.Value);
        var objectArgs = selected == null
            ? new ChangeEventArgs { Value = null }
            : new ChangeEventArgs { Value = selected.Value };
        await NotifyInput(objectArgs);
    }

    private async Task NotifyChangeByKey(ChangeEventArgs args)
    {
        var selected = InternalOptions.FirstOrDefault(e => e.Key == (string?)args.Value);
        var objectArgs = selected == null
            ? new ChangeEventArgs { Value = null }
            : new ChangeEventArgs { Value = selected.Value };
        await NotifyChange(objectArgs);
    }

}
