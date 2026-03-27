namespace ExtraDry.Blazor;

/// <summary>
/// Generates a generic form for the creation and updating of a model. Form layout is based on the
/// attributes of the fields of the model, as well as the optional view model.
/// </summary>
public partial class DryForm<T>(
    ILogger<DryForm<T>> Logger)
    : ComponentBase, IExtraDryComponent, IDryForm
    where T : class
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = "";

    /// <summary>
    /// The target model for the form that is being edited.
    /// </summary>
    [Parameter, EditorRequired]
    public T? Model { get; set; }

    public object? UntypedModel {
        get => Model;
    }

    [Parameter]
    public object? Decorator { get; set; }

    /// <inheritdoc cref="Blazor.EditMode" />
    [Parameter]
    public EditMode EditMode { get; set; } = EditMode.Update;

    /// <summary>
    /// A comma-separate list of fieldsets that are displayed by the form. This is useful to limit
    /// the form to a subset and allow others to be rendered separately. 
    /// </summary>
    [Parameter]
    public string Fieldsets { get; set; } = "*";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string CommandCategory { get; set; } = "*";

    /// <summary>
    /// An event callback that is invoked whenever any field value in the form changes. The event
    /// is relayed through each <see cref="Forms.DryFieldset" /> from the constituent DryInput
    /// components. The callback fires after the bound model property has been updated, ensuring
    /// the latest form state is available to the handler.
    /// </summary>
    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    public string ModelNameSlug => Slug.ToSlug(FormDescription?.ViewModelDescription?.ModelDisplayName ?? "");

    public DecoratorInfo? Description { get; set; }

    public FormDescription? FormDescription { get; set; }

    internal string GetEntityInfoCaption()
    {
        string caption = string.Empty;
        if(Decorator is IViewModelCaption viewModelCaption) {
            caption = viewModelCaption.Caption(Decorator);
        }
        return string.IsNullOrEmpty(caption) ? Description?.ModelDisplayName ?? string.Empty : caption;
    }

    protected override void OnParametersSet()
    {
        FieldsetNames = Fieldsets.Split(',');
        if(Decorator == null) {
            Logger.LogConsoleError("DryForm requires a Decorator");
            return;
        }
        Description ??= new DecoratorInfo(typeof(T), Decorator);
        if(Model is not null) {
            Description = new DecoratorInfo(Model.GetType(), Decorator); // override the Description with the actual Description when we have the Model, which will account for polymorphism issues
            FormDescription = new FormDescription(Description, Model);
        }
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-form", ModelNameSlug, CssClass);

    private string[] FieldsetNames { get; set; } = [];

    private IEnumerable<FormFieldset> VisibleFieldsets => (FieldsetNames.Any(e => e == "*")
        ? FormDescription?.Fieldsets
        : FormDescription?.Fieldsets.Where(e => FieldsetNames.Contains(e.Name, StringComparer.OrdinalIgnoreCase)))
        ?? [];
}
