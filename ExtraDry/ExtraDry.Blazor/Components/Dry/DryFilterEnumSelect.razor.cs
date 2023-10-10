namespace ExtraDry.Blazor;

/// <summary>
/// A filter component used by the DryFilter to a drop-down dialog that presents filter options
/// for an enum property.  
/// </summary>
public partial class DryFilterEnumSelect : ComponentBase, IExtraDryComponent, IDisposable {

    /// <summary>
    /// The property that is used to present the options for the enum select.
    /// </summary>
    [Parameter]
    public PropertyDescription? Property { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="FlexiSelect{TItem}.Placeholder" />
    [Parameter]
    public string Placeholder { get; set; } = "Select...";

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

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
            EnumValues = Property.GetDiscreteValues();
            PageQueryBuilder.OnChanged += PageQueryBuilder_OnChanged;
        }
    }

    private void PageQueryBuilder_OnChanged(object? sender, EventArgs e)
    {
        if(filterInSync && !FiltersMatchValues() && Values != null && Filter != null) {
            // component thinks filters in sync but they don't match, process incoming changes.
            Values.Clear();
            foreach(var value in Filter.Values) {
                var vd = EnumValues.FirstOrDefault(e => e.Title == value);
                if(vd != null) {
                    Values.Add(vd);
                }
            }
        }
    }

    /// <summary>
    /// Determines if the external filter matches the internal state of this component.
    /// Typically used to see if another component has update the PageQueryBuilder in a fashion
    /// that would require this component to be updated.
    /// </summary>
    private bool FiltersMatchValues() {
        // Quick a common check based on count of values...
        var filterCount = Filter?.Values?.Count ?? 0;
        var valueCount = Values?.Count ?? 0;
        if(filterCount != valueCount) {
            return false;
        }
        // Same count, must check them all, but will be in the same order so use that fact.
        for(int i = 0; i < filterCount; ++i) {
            var externalValue = Filter?.Values[i];
            var internalValue = Values?[i].Title;
            if(internalValue != externalValue) {
                return false;
            }
        }
        // Looks clear...
        return true;
    }             

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if(PageQueryBuilder != null) {
            PageQueryBuilder.OnChanged -= PageQueryBuilder_OnChanged;
        }
    }

    private Task SyncWithPageQuery()
    {
        Filter?.Values.Clear();
        if(Values != null) {
            Filter?.Values?.AddRange(Values.Select(e => e.Title));
        }
        filterInSync = false;
        PageQueryBuilder?.NotifyChanged();
        filterInSync = true;
        return Task.CompletedTask;
    }

    private bool filterInSync = true;

    private IList<ValueDescription> EnumValues { get; set; } = Array.Empty<ValueDescription>();

    private ValueDescription? Value { get; set; }

    private List<ValueDescription>? Values { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-field-filter", "filter-enum", CssClass, PropertyNameSlug, PropertyTypeSlug);

    private string PropertyNameSlug => Property?.Property?.Name?.Split('.')?.Last()?.ToLowerInvariant() ?? string.Empty;

    private string PropertyTypeSlug => Property?.Property?.PropertyType?.Name?.Split('.')?.Last()?.ToLowerInvariant() ?? string.Empty;

    /// <summary>
    /// The filter from the PageQueryBuilder, also contains a list like the Selection property, but
    /// is only updated just before the PageQueryBuilder is asked to notify all observers.
    /// </summary>
    private EnumFilterBuilder? Filter { get; set; }

}
