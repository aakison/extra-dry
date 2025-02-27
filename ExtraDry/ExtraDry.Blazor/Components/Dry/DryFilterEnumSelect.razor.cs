﻿using ExtraDry.Blazor.Components.Internal;

namespace ExtraDry.Blazor;

/// <summary>
/// A filter component used by the DryFilter to a drop-down dialog that presents filter options for
/// an enum property.
/// </summary>
public partial class DryFilterEnumSelect : ComponentBase, IExtraDryComponent, IDisposable
{
    /// <summary>
    /// The property that is used to present the options for the enum select.
    /// </summary>
    [Parameter, EditorRequired]
    public PropertyDescription Property { get; set; } = null!;

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = "";

    /// <inheritdoc cref="FlexiSelect{TItem}.Placeholder" />
    [Parameter]
    public string Placeholder { get; set; } = "Select...";

    [Parameter, EditorRequired]
    public object Decorator { get; set; } = null!;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private QueryBuilderAccessor? QueryBuilderAccessor { get; set; }

    /// <summary>
    /// When parameters are set, check if the PageQuery has a filter that matches our property by
    /// name. If not, create one and add it to the PageQuery.
    /// </summary>
    /// <remarks>Multiple filters mapped to the same PageQuery are not supported.</remarks>
    protected override void OnParametersSet()
    {
        if(QueryBuilderAccessor == null) {
            QueryBuilderAccessor = new QueryBuilderAccessor(Decorator);
            QueryBuilderAccessor.QueryBuilder.OnChanged += PageQueryBuilder_OnChanged;
        }
        Filter = QueryBuilderAccessor.QueryBuilder.Filters
            .FirstOrDefault(e => string.Equals(e.FilterName, Property.Property.Name, StringComparison.OrdinalIgnoreCase))
            as EnumFilterBuilder;
        if(Filter == null) {
            Filter = new EnumFilterBuilder { FilterName = Property.Property.Name };
            QueryBuilderAccessor.QueryBuilder.Filters.Add(Filter);
        }
        else {
            //There is an existing filter, check if we need to sync the FilterSelect Values
            SyncValues();
        }
        EnumValues = Property.GetDiscreteValues();
    }

    private void PageQueryBuilder_OnChanged(object? sender, EventArgs e)
    {
        if(filterInSync && !FiltersMatchValues() && Values != null && Filter != null) {
            // component thinks filters in sync but they don't match, process incoming changes.
            SyncValues();
        }
    }

    private void SyncValues()
    {
        if(Filter == null) { return; }
        Values ??= [];

        Values.Clear();
        foreach(var value in Filter.Values) {
            var vd = EnumValues.FirstOrDefault(e => string.Equals(e.Title, value, StringComparison.OrdinalIgnoreCase));
            if(vd != null) {
                Values.Add(vd);
            }
        }

        StateHasChanged();
    }

    /// <summary>
    /// Determines if the external filter matches the internal state of this component. Typically
    /// used to see if another component has update the PageQueryBuilder in a fashion that would
    /// require this component to be updated.
    /// </summary>
    private bool FiltersMatchValues()
    {
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
        if(QueryBuilderAccessor != null) {
            QueryBuilderAccessor.QueryBuilder.OnChanged -= PageQueryBuilder_OnChanged;
            QueryBuilderAccessor = null;
        }
    }

    private Task SyncWithPageQuery()
    {
        Filter?.Values.Clear();
        if(Values != null) {
            Filter?.Values?.AddRange(Values.Select(e => e.Title));
        }
        filterInSync = false;
        QueryBuilderAccessor?.QueryBuilder.NotifyChanged();
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
