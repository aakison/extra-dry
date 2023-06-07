namespace ExtraDry.Blazor;

/// <summary>
/// A filter component used by the DryFilter to a drop-down dialog that presents filter options
/// for an enum property.  
/// </summary>
public partial class DryFilterEnumSelect : ComponentBase, IExtraDryComponent {

    /// <summary>
    /// The property that is used to present the options for the enum select.
    /// </summary>
    [Parameter]
    public PropertyDescription? Property { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    /// <inheritdoc cref="DryPageQueryView.PageQueryBuilder" />
    [CascadingParameter]
    public PageQueryBuilder? PageQueryBuilder { get; set; }

    /// <summary>
    /// When parameters are set, check if the PageQuery has a filter that matches our property
    /// by name.  If not, create one and add it to the PageQuery.
    /// </summary>
    /// <remarks>
    /// Multiple filters mapped to the same PageQuery are not supported.
    /// </remarks>
    protected override void OnParametersSet()
    {
        if(Property != null && PageQueryBuilder != null) {
            Filter = PageQueryBuilder.Filters
                .FirstOrDefault(e => string.Equals(e.FilterName, Property.Property.Name, StringComparison.OrdinalIgnoreCase)) 
                as EnumFilterBuilder;
            if(Filter == null) {
                Filter = new EnumFilterBuilder { FilterName = Property.Property.Name };
                PageQueryBuilder.Filters.Add(Filter);
            }
        }
    }

    /// <summary>
    /// Expands or collapses the dialog box programmatically.
    /// </summary>
    public async Task ToggleForm()
    {
        await Expandable.Toggle();
        StateHasChanged();
    }

    /// <summary>
    /// Focus management is done using a div and events, the OnFocusIn simply stores the state
    /// that the dialog should be shown.
    /// </summary>
    protected Task OnFocusIn(FocusEventArgs args)
    {
        shouldCollapse = false;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Focus management to determine if the dialog should collapse is non-trivial.  The OnFocusOut
    /// event fires when focus changes from one child of the dialog to another child of the dialog.
    /// A one-frame delay allows the in/out events to propagate so if the OnFocusOut is called just
    /// before the OnFocusIn is called
    /// </summary>
    protected async Task OnFocusOut(FocusEventArgs args)
    {
        shouldCollapse = true;
        // wait and see if we should ignore the out because we're switching focus within control.
        await Task.Delay(1);
        if(shouldCollapse) {
            SyncPageQueryBuilder();
            await Expandable.Collapse();
            shouldCollapse = false;
        }
    }

    /// <summary>
    /// When a list item is checked/unchecked chagne the selection set.  However, this is not 
    /// typically enough to warrent a server roundtrip, wait until losing focus or other major
    /// event before changing.
    /// </summary>
    protected void OnChange(ChangeEventArgs args, ValueDescription value)
    {
        var key = value.Key.ToString();
        if(key == null) {
            return;
        }
        if(args?.Value?.Equals(true) ?? false) {
            if(!Selection.Contains(key)) {
                Selection.Add(key);
            }
        }
        else {
            if(Selection.Contains(key)) {
                Selection.Remove(key);
            }
        }
    }

    /// <summary>
    /// If the user selects Enter while in the dialog, apply the filter immediately.  Allows user
    /// to use tab/space to edit dialog box and enter to get results without
    /// </summary>
    protected Task OnKeyDown(KeyboardEventArgs args)
    {
        if(args.Key == "Enter") {
            SyncPageQueryBuilder();
        }
        return Task.CompletedTask;
    }

    private bool shouldCollapse = false;

    private void SyncPageQueryBuilder()
    {
        if(PageQueryBuilder != null) {
            Filter?.Values?.Clear();
            Filter?.Values?.AddRange(Selection);
            PageQueryBuilder.NotifyChanged();
        }
    }

    private string PropertyLabel => Property?.Property?.Name ?? "Missing Property Field";

    private DryExpandable Expandable { get; set; } = null!; // set in page-side partial

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-filter", "filter-enum", CssClass, PropertyNameSlug, PropertyTypeSlug, PopulatedClass);

    private string PropertyNameSlug => Property?.Property?.Name?.Split('.')?.Last()?.ToLowerInvariant() ?? string.Empty;

    private string PropertyTypeSlug => Property?.Property?.PropertyType?.Name?.Split('.')?.Last()?.ToLowerInvariant() ?? string.Empty;

    private string PopulatedClass => Selection.Any() ? "active" : "inactive";

    /// <summary>
    /// The list of currently selected items, updated on every click.
    /// </summary>
    private List<string> Selection { get; } = new();

    /// <summary>
    /// The filter from the PageQueryBuilder, also contains a list like the Selection property, but
    /// is only updated just before the PageQueryBuilder is asked to notify all observers.
    /// </summary>
    private EnumFilterBuilder? Filter { get; set; }

}
