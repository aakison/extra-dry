using ExtraDry.Blazor.Components.Internal;

namespace ExtraDry.Blazor;

/// <summary>
/// Generates a generic form for the creation and updating of a model.  Form layout is based on 
/// the attributes of the fields of the model, as well as the optional view model.
/// </summary>
public partial class DryForm<T> : ComponentBase, IExtraDryComponent {

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The target model for the form that is being edited.
    /// </summary>
    [Parameter, EditorRequired]
    public T? Model { get; set; }

    [Parameter]
    public object? ViewModel { get; set; }

    /// <inheritdoc cref="Blazor.EditMode"/>
    [Parameter]
    public EditMode EditMode { get; set; } = EditMode.Update;

    /// <summary>
    /// Represents the number of fieldsets that are rendered in the first collection of fieldsets.
    /// The remainder are rendered in a second collection of fieldsets.  CSS styles can render 
    /// these two separately, e.g. my making the second set scrollable.
    /// </summary>
    [Parameter]
    public int FixedFieldsets { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    [Inject]
    private ILogger<DryForm<T>> Logger { get; set; } = null!;

    internal string GetEntityInfoDisplayName()
    {
        string displayName = string.Empty;
        if(ViewModel is IViewModelDisplayName viewModelDisplayName) {
            displayName = viewModelDisplayName.DisplayName(ViewModel);
        }
        return string.IsNullOrEmpty(displayName) ? Description?.ModelDisplayName ?? string.Empty : displayName;
    }

    protected override void OnParametersSet()
    {
        if(ViewModel == null) {
            Logger.LogConsoleError("DryForm requires a ViewModel");
            return;
        }
        Description ??= new ViewModelDescription(typeof(T), ViewModel);
        if(Model != null) {
            Description = new ViewModelDescription(Model.GetType(), ViewModel); // override the Description with the actual Description when we have the Model, which will account for polymorphism issues
            FormDescription = new FormDescription(Description, Model);
        }
    }

    internal string ModelNameSlug => Slug.ToSlug(FormDescription?.ViewModelDescription?.ModelDisplayName ?? "");

    internal ViewModelDescription? Description { get; set; }

    internal FormDescription? FormDescription { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-form", ModelNameSlug, CssClass);

    private List<string> AlertMessages { get; set; } = new();
}
