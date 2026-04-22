namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents a toggle field (checkbox) for boolean and numeric values, including support for
/// indeterminate state when <typeparamref name="TValue"/> is a nullable type (e.g. <c>bool?</c>,
/// <c>int?</c>). For numeric types, <c>true</c> maps to <c>1</c> and <c>false</c> maps to <c>0</c>.
/// </summary>
public partial class ToggleField<TValue> : FieldBase<TValue>
{
    [Inject]
    private ExtraDryJavascriptModule Module { get; set; } = null!;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "checkbox", ReadOnlyCss, CssClass);

    private bool Indeterminate => Value is null;

    private bool Checked => Value switch {
        bool b => b,
        null => false,
        _ => Convert.ToBoolean(Value, CultureInfo.InvariantCulture),
    };

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
        args.Value = ConvertToTValue(args.Value);
        await NotifyChange(args);
    }

    private async Task HandleInput(ChangeEventArgs args)
    {
        args.Value = ConvertToTValue(args.Value);
        await NotifyInput(args);
    }

    private static TValue ConvertToTValue(object? value)
    {
        bool? boolValue = value is bool b ? b : null;
        var targetType = typeof(TValue);
        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if(boolValue == null) {
            return default!; // null for nullable types, default(0/false) for non-nullable
        }
        if(underlyingType == typeof(bool)) {
            return (TValue)(object)boolValue.Value;
        }
        // Numeric types: true → 1, false → 0
        try {
            return (TValue)Convert.ChangeType(boolValue.Value ? 1 : 0, underlyingType, CultureInfo.InvariantCulture);
        }
        catch {
            return default!;
        }
    }
}
