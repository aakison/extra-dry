using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a single select dropdown list.  Prefer the use of <see cref="DryInput{T}"/> 
/// instead of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputSingleSelect<T> : DryInputBase<T> {

    /// <summary>
    /// Set of values to select from, any object can be used and the ToString values are displayed.
    /// </summary>
    [Parameter, EditorRequired]
    public List<object>? Values { get; set; } 

    protected override void OnParametersSet()
    {
        if(Model != null) {
            SelectedValue = Property?.GetValue(Model);
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

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private object? SelectedValue { get; set; }

}
