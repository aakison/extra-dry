using System.Reflection;

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
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The target model for the form that is being edited.
    /// </summary>
    [Parameter, EditorRequired]
    public T? Model { get; set; }

    /// <summary>
    /// Determines if the form has a title with descriptive information
    /// </summary>
    [Parameter]
    public bool ShowTitle { get; set; } = true;

    public object? UntypedModel {
        get => Model;
    }

    [Parameter]
    public object? Decorator { get; set; }

    /// <inheritdoc cref="Blazor.EditMode" />
    [Parameter]
    public EditMode EditMode { get; set; } = EditMode.Update;

    /// <summary>
    /// Represents the number of fieldsets that are rendered in the first collection of fieldsets.
    /// The remainder are rendered in a second collection of fieldsets. CSS styles can render these
    /// two separately, e.g. my making the second set scrollable.
    /// </summary>
    [Parameter]
    public int FixedFieldsets { get; set; }

    /// <summary>
    /// A comma-separate list of fieldsets that are not displayed by the form. This is useful if
    /// those fieldsets are not used for the view (e.g. no advanced settings during creation), or
    /// if an alternate visualization is used (e.g. using a DryFieldset in tab).
    /// </summary>
    [Parameter]
    public string HiddenFieldsets { get; set; } = "";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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
        HiddenFieldsetNames = HiddenFieldsets.Split(',');
        if(Decorator == null) {
            Logger.LogConsoleError("DryForm requires a Decorator");
            return;
        }
        Description ??= new DecoratorInfo(typeof(T), Decorator);
        if(Model != null) {
            Description = new DecoratorInfo(Model.GetType(), Decorator); // override the Description with the actual Description when we have the Model, which will account for polymorphism issues
            FormDescription = new FormDescription(Description, Model);
        }
    }

    private bool DisplayTitle => ShowTitle;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-form", ModelNameSlug, CssClass);

    private List<string> AlertMessages { get; set; } = [];

    private IEnumerable<FormFieldset> DisplayFixedFieldsets => VisibleFieldsets.Take(FixedFieldsets) ?? [];

    private IEnumerable<FormFieldset> DisplayVariableFieldsets => VisibleFieldsets.Skip(FixedFieldsets) ?? [];

    private string[] HiddenFieldsetNames { get; set; } = [];

    private IEnumerable<FormFieldset> VisibleFieldsets => 
        HiddenFieldsets == "*" 
        ? [] 
        : FormDescription?.Fieldsets.Where(e => !HiddenFieldsetNames.Contains(e.Name, StringComparer.OrdinalIgnoreCase)) 
        ?? [];
}
