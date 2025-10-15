
namespace ExtraDry.Blazor.Components;

/// <summary>
/// A DRY wrapper around an option field (dropdown/select). Prefer the use of DryField instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
/// <typeparam name="TModel">The model type containing the property to edit.</typeparam>
public partial class DryOptionField<TModel> : DryFieldBase<TModel> where TModel : class
{
    protected override async Task OnParametersSetAsync()
    {
        if(Model == null || Property == null) {
            return;
        }
        await LoadOptionsForPropertyAsync();
        var value = Property.GetValue(Model);
        Value = Options.FirstOrDefault(e => e.Value?.Equals(value) ?? value == null);
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "option", ReadOnlyCss, CssClass);

    private Option? Value { get; set; }

    private IList<Option> Options { get; set; } = [];

    private async Task HandleChange(ChangeEventArgs args)
    {
        UpdateModelProperty(args.Value);
        var valid = ValidateProperty();
        await OnChange.InvokeAsync(args);
        await InvokeOnValidationAsync(valid);
    }

    private async Task HandleInput(ChangeEventArgs args)
    {
        UpdateModelProperty(args.Value);
        await OnInput.InvokeAsync(args);
    }

    private void UpdateModelProperty(object? value)
    {
        Console.WriteLine($"Updating property {Property?.Property.Name} on {typeof(TModel).Name} to value {value}");
        if(Property == null || Model == null) {
            return;
        }
        if(value is Option optionValue) {
            Console.WriteLine($"Option value selected: {optionValue.Value} ({optionValue.Title})");
            Value = optionValue;
            value = optionValue.Value;
        }
        Console.WriteLine($"Setting property {Property.Property.Name} on {typeof(TModel).Name} to value {value}");
        Property.SetValue(Model, value);
        StateHasChanged();
    }

    public class Option : IResourceIdentifiers
    {
        public required Guid Uuid { get; set; }
        public string Slug { get => ""; set { } }
        public required string Title { get; set; }
        public required object Value { get; set; }
    }

    private static Func<object, string> TitleFunc => e => (e as IResourceIdentifiers)?.Title ?? e?.ToString() ?? "--empty--";

    private static Func<object, Guid> UuidFunc => e => (e as IUniqueIdentifier)?.Uuid ?? Guid.NewGuid();

    private async Task LoadOptionsForPropertyAsync()
    {
        if(Property == null) {
            return;
        }
        if(Property.PropertyType.IsEnum) {
            var enumValues = DataConverter.EnumValues(Property.PropertyType);
            Options = [.. enumValues.Select(e => new Option { Uuid = UuidFunc(e), Title = DataConverter.DisplayEnum(e), Value = e })];
            return;
        }
        else {
            var untypedOptionProvider = typeof(IOptionProvider<>);
            var propertyType = Property.InputType;
            if(propertyType.IsAssignableTo(typeof(IList))) {
                propertyType = propertyType.GetGenericArguments().FirstOrDefault();
                if(propertyType == null) {
                    Logger.LogError("Could not determine generic argument of IList for property {PropertyName} on {ModelType}", Property.Property.Name, typeof(TModel).Name);
                    return;
                }
            }
            var typedOptionProvider = untypedOptionProvider.MakeGenericType(propertyType);
            var optionProvider = ScopedServices.GetService(typedOptionProvider);
            if(optionProvider != null) {
                var method = typedOptionProvider.GetMethod("GetItemsAsync");
                var token = new CancellationTokenSource().Token;
                dynamic task = method!.Invoke(optionProvider, [token])!;
                var optList = (await task).Items as ICollection;
                var options = optList?.Cast<object>()?.ToList() ?? [];
                //if(options.FirstOrDefault() is IndexOutOfRangeException resource) {
                Options = [.. options.Select(e => new Option { Uuid = UuidFunc(e), Title = TitleFunc(e), Value = e })];
                //}
                //else {
                //    LookupProviderOptions = options
                //        .Select((e, i) => new { Key = i, Item = e })
                //        .ToDictionary(e => e.Key.ToString(CultureInfo.InvariantCulture), e => e.Item);
                //}
            }
            else {
                Logger.LogMissingOptionProvider(Property.InputType.Name);
            }
        }
    }
}
