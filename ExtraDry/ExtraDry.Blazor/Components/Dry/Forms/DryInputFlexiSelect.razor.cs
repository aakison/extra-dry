using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a text input field. Prefer the use of <see cref="DryInput{T}" /> instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputFlexiSelect<T>
    : DryInputBase<T>
    where T : class
{
    [Parameter]
    public bool ReadOnly { get; set; }

    protected override void OnParametersSet()
    {
        EnumValues = Property.GetDiscreteValues();
        var objValue = Property.GetValue(Model);
        Value = EnumValues.FirstOrDefault(e => e.Key.Equals(objValue));
    }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", ReadOnlyCss, CssClass);

    private IList<ValueDescription> EnumValues { get; set; } = Array.Empty<ValueDescription>();

    private bool MultiSelect => Property.HasArrayValues;

    private ValueDescription? Value { get; set; }

    private List<ValueDescription?>? Values { get; set; }

    private async Task HandleChange(DialogEventArgs args)
    {
        //var value = args.Value;
        Logger.LogWarning("Value Saved: {Value}", Value);
        Property.SetValue(Model, Value?.Key);
        var valid = ValidateProperty();

        await InvokeOnChangeAsync(Value?.Key);
        await InvokeOnValidationAsync(valid);
    }
}
