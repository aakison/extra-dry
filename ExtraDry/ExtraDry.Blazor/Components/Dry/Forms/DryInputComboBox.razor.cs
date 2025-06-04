using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a combo box input field. Prefer the use of <see cref="DryInput{T}" /> instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputComboBox<T>(
    IServiceProvider services)
    : DryInputBase<T>
    where T : class
{
    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public IEnumerable<string>? Items { get; set; }

    [Parameter]
    public IListService<string>? ItemsSource { get; set; }

    protected override void OnInitialized()
    {
        if(Property?.Options == null) {
            return;
        }
        var options = services.GetService(Property.Options.ProviderType) as IListService<string>
            ?? Activator.CreateInstance(Property.Options.ProviderType) as IListService<string>;
        if(options is null) {
            Logger.LogWarning("Property {PropertyName} has an options provider of type {ProviderType} but it is not a valid IOptionProvider<string>.", Property.Property.Name, Property.Options.ProviderType);
        }
        ResolvedItemsSource = options ?? ItemsSource;
    }

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        Value = Property.DisplayValue(Model);
    }

    private IListService<string>? ResolvedItemsSource { get; set; }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "text", ReadOnlyCss, CssClass);

    private string Value { get; set; } = "";

    private async Task InvokeOnChange(ChangeEventArgs args)
    {
        await OnChange.InvokeAsync(args);
    }

    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }
        var value = args.Value;
        Property.SetValue(Model, value);
        var valid = ValidateProperty();

        await InvokeOnChangeAsync(value);
        await InvokeOnValidationAsync(valid);
    }
}
