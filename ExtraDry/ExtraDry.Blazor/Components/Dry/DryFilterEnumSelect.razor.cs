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
            EnumValues = Property.GetDiscreteValues();
        }
    }

    private Task SyncWithPageQuery()
    {
        Filter?.Values.Clear();
        if(Values != null) {
            Filter?.Values?.AddRange(Values.Select(e => e.Title));
        }
        PageQueryBuilder?.NotifyChanged();
        return Task.CompletedTask;
    }

    private IList<ValueDescription> EnumValues { get; set; } = Array.Empty<ValueDescription>();

    private ValueDescription? Value { get; set; }

    private List<ValueDescription>? Values { get; set; }

    //private void SyncPageQueryBuilder()
    //{
    //    if(PageQueryBuilder != null) {
    //        Filter?.Values?.Clear();
    //        Filter?.Values?.AddRange(Selection);
    //        PageQueryBuilder.NotifyChanged();
    //    }
    //}

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "filter-enum", CssClass, PropertyNameSlug, PropertyTypeSlug);

    private string PropertyNameSlug => Property?.Property?.Name?.Split('.')?.Last()?.ToLowerInvariant() ?? string.Empty;

    private string PropertyTypeSlug => Property?.Property?.PropertyType?.Name?.Split('.')?.Last()?.ToLowerInvariant() ?? string.Empty;

    /// <summary>
    /// The filter from the PageQueryBuilder, also contains a list like the Selection property, but
    /// is only updated just before the PageQueryBuilder is asked to notify all observers.
    /// </summary>
    private EnumFilterBuilder? Filter { get; set; }

}
