using System.Reflection;

namespace ExtraDry.Blazor;

/// <summary>
/// Generates a generic form for the creation and updating of a model. Form layout is based on the
/// attributes of the fields of the model, as well as the optional view model.
/// </summary>
public partial class DryForm<T>
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

    public object? UntypedModel {
        get => Model;
    }

    [Parameter]
    public object? ViewModel { get; set; }

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
        if(ViewModel is IViewModelCaption viewModelCaption) {
            caption = viewModelCaption.Caption(ViewModel);
        }
        return string.IsNullOrEmpty(caption) ? Description?.ModelDisplayName ?? string.Empty : caption;
    }

    protected override void OnParametersSet()
    {
        if(ViewModel == null) {
            Logger.LogConsoleError("DryForm requires a ViewModel");
            return;
        }
        Description ??= new DecoratorInfo(typeof(T), ViewModel);
        if(Model != null) {
            Description = new DecoratorInfo(Model.GetType(), ViewModel); // override the Description with the actual Description when we have the Model, which will account for polymorphism issues
            FormDescription = new FormDescription(Description, Model);
        }
    }

    [Inject]
    private ILogger<DryForm<T>> Logger { get; set; } = null!;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-form", ModelNameSlug, CssClass);

    private List<string> AlertMessages { get; set; } = [];
}
