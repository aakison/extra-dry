#nullable disable

using System.Collections;

namespace ExtraDry.Blazor;

public partial class DryInput<T> : OwningComponentBase, IDisposable {

    [Parameter]
    public T Model { get; set; }

    [Parameter]
    public PropertyDescription Property { get; set; }

    [Parameter]
    public string PropertyName { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs>? OnChange { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    protected async override Task OnInitializedAsync()
    {
        Property ??= new PropertyDescription(typeof(T).GetProperty(PropertyName));
        if(Property?.Rules?.UpdateAction == RuleAction.Block) {
        }
        else if(Property?.HasTextRepresentation == false) {
            await FetchLookupProviderOptions();
        }
    }

    private Dictionary<string, object> LookupProviderOptions { get; set; }

    private List<object> LookupValues => LookupProviderOptions.Values.ToList();

    private bool RulesAllowUpdate => Property.Rules?.UpdateAction switch {
            RuleAction.Block => false,
            RuleAction.Ignore => false,
            _ => true,
        };

    private bool Editable => EditMode == EditMode.Create || EditMode == EditMode.Update && RulesAllowUpdate;

    private bool ReadOnly => !Editable;

    private string Value => Property.DisplayValue(Model);

    private string validationMessage;

    private bool valid = true;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, Property.DisplayClass, StateCss, ValidCss);

    private string SizeClass => Property.Size.ToString().ToLowerInvariant();

    private bool ShowDescription { get; set; }

    private bool HasDescription => Property.HasDescription;

    private async Task FetchLookupProviderOptions()
    {
        var untypedOptionProvider = typeof(IOptionProvider<>);
        var propertyType = Property.Property.PropertyType;
        if(propertyType.IsAssignableTo(typeof(IList))) {
            propertyType = propertyType.GetGenericArguments().FirstOrDefault();
        }
        var typedOptionProvider = untypedOptionProvider.MakeGenericType(propertyType);
        var optionProvider = ScopedServices.GetService(typedOptionProvider);
        if(optionProvider != null) {
            var method = typedOptionProvider.GetMethod("GetItemsAsync");
            var token = new CancellationTokenSource().Token;
            dynamic task = method.Invoke(optionProvider, new object[] { token });
            var optList = (await task).Items as ICollection;
            var options = optList.Cast<object>().ToList();
            LookupProviderOptions = options.Select((e, i) => new { Key = i, Item = e }).ToDictionary(e => e.Key.ToString(CultureInfo.InvariantCulture), e => e.Item);
        }
        else {
            Logger.LogMissingOptionProvider(Property?.Property?.PropertyType?.Name);
        }
    }

    private string TextDescription => Property.Description;

    private string StateCss => (Editable, Property.IsRequired) switch {
        (true, true) => "required",
        (true, false) => "optional",
        (false, _) => "readonly"
    };

    private string ValidCss => valid ? " valid" : " invalid";

    private string HtmlDescription => TextDescription.Replace("-", "&#8209;"); // non-breaking-hyphen.

    private void ToggleDescription(MouseEventArgs args)
    {
        ShowDescription = !ShowDescription;
    }

    private async Task HandleChange(ChangeEventArgs args)
    {
        Console.WriteLine("HandleChange");
        Console.WriteLine($"Changed to: {args} / {args.Value}");
        var value = args.Value;
        if(LookupProviderOptions != null && value is string strValue) {
            value = LookupProviderOptions[strValue];
        }
        Console.WriteLine($"Model: {Model} to Value: {value}");
        Property.SetValue(Model, value);
        Validate();
        var task = OnChange?.InvokeAsync(args);
        if(task != null) {
            await task;
        }
    }

    private async Task HandleClick(object selectValue)
    {
        Console.WriteLine("Changed");
        var value = selectValue;
        //if(LookupProviderOptions != null && value is string strValue) {
        //    value = LookupProviderOptions[strValue];
        //}
        Console.WriteLine($"Model: {Model} to Value: {value}");
        Property.SetValue(Model, value);
        Validate();
        // Ignore that it's a physical click and treat like value change for listeners.
        var changeEventArgs = new ChangeEventArgs { Value = value };
        var task = OnChange?.InvokeAsync(changeEventArgs);
        if(task != null) {
            await task;
        }
        StateHasChanged();
    }

    private void Validate()
    {
        var validator = new DataValidator();
        if(validator.ValidateProperties(Model, Property.Property.Name)) {
            validationMessage = "";
            valid = true;
        }
        else {
            validationMessage = validator.Errors.First().ErrorMessage;
            valid = false;
        }
    }

}
