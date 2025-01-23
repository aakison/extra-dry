using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a single select dropdown list. Prefer the use of <see cref="DryInput{T}"
/// /> instead of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputSingleSelect<T>
    : DryInputBase<T>, IDryInput<T>, IExtraDryComponent
    where T : class
{
    /// <summary>
    /// Set of values to select from, any object can be used and the display text is either
    /// IResourceIdentifier.Title or object.ToString() value.
    /// </summary>
    [Parameter, EditorRequired]
    public List<object> Values { get; set; } = null!;

    protected override void OnParametersSet()
    {
        SelectedValue = Property.GetValue(Model);
        if(SelectedValue is Guid uuid && Property.InputType.IsClass) {
            SelectedValue = Values.SingleOrDefault(e => (e as IResourceIdentifiers)?.Uuid == uuid);
        }
    }

    private async Task SelectOption(ChangeEventArgs args)
    {
        if(Values == null || !int.TryParse(args.Value as string, out var index)) {
            return; // selected blank line
        }
        SelectedValue = Values[index];
        if(Model != null) {
            Property?.SetValue(Model, SelectedValue);
            await InvokeOnChange(args);
        }
    }

    private async Task InvokeOnChange(ChangeEventArgs args)
    {
        await OnChange.InvokeAsync(args);
    }

    private bool ReadOnly => EditMode == EditMode.ReadOnly;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "select", ReadOnlyCss, CssClass);

    private object? SelectedValue { get; set; }
}
