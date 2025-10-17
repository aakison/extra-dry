namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents a toggle field (checkbox) for boolean values, including support for indeterminate state.
/// </summary>
public partial class ToggleField : FieldBase<bool?>
{
    [Inject]
    private ExtraDryJavascriptModule Module { get; set; } = null!;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "checkbox", ReadOnlyCss, CssClass);

    private bool Indeterminate => Value == null;

    private bool Checked => Value == true;

    private bool jsIndeterminate;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        //await base.OnAfterRenderAsync(firstRender);

        if(jsIndeterminate != Indeterminate) {
            // Only do interop if we need to change.

            await Module.InvokeVoidAsync("ToggleField_SetIndeterminate", InputId, Indeterminate);
            jsIndeterminate = Indeterminate;
        }
    }

    private async Task HandleChange(ChangeEventArgs args)
    {
        bool? newValue = null;
        if(args.Value is bool boolValue) {
            newValue = boolValue;
        }
        var updatedArgs = new ChangeEventArgs { Value = newValue };
        await NotifyChange(updatedArgs);
    }

    private async Task HandleInput(ChangeEventArgs args)
    {
        bool? newValue = null;
        if(args.Value is bool boolValue) {
            newValue = boolValue;
        }
        var updatedArgs = new ChangeEventArgs { Value = newValue };
        await NotifyInput(updatedArgs);
    }
}
