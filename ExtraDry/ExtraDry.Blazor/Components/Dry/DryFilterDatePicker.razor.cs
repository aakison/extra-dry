﻿using ExtraDry.Blazor.Components.Internal;

namespace ExtraDry.Blazor;

/// <summary>
/// A filter component used by the DryFilter to provide a drop-down dialog that presents filtering
/// options for a DateTime property.
/// </summary>
/// <remarks>
/// The time portion of the filter has not been implemented at this point, but will be added when
/// required.
/// </remarks>
public partial class DryFilterDatePicker : ComponentBase, IExtraDryComponent, IDisposable
{
    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <summary>
    /// The property that the filter is being applied to.
    /// </summary>
    [Parameter, EditorRequired]
    public PropertyDescription Property { get; set; } = null!;

    /// <inheritdoc cref="IComments.Placeholder" />
    [Parameter]
    public string Placeholder { get; set; } = "Select...";

    /// <inheritdoc cref="Button.Affordance" />
    [Parameter]
    public string Affordance { get; set; } = "select";

    [Parameter]
    public string PreviousIcon { get; set; } = "back";

    [Parameter]
    public string NextAffordance { get; set; } = "forward";

    [Parameter, EditorRequired]
    public object Decorator { get; set; } = null!;

    private QueryBuilderAccessor? QueryBuilderAccessor { get; set; }

    [Inject]
    private ILogger<DryFilterDatePicker> Logger { get; set; } = null!;

    protected MiniDialog? MiniDialog { get; set; }

    private bool DisplayCaption => Selected == null;

    private bool CanChangeDate => Selected != null && Selected.Type != TimeIntervalType.Static;

    private DateTimeFilterBuilder? Filter { get; set; }

    private List<TimeIntervalGroup> TimeIntervalGroups { get; } = [
        new() {
            Title = "Relative dates",
            TimeIntervals = [
                new ("Before today", null, DateTime.Now),
                new (TimeIntervalType.Days, -7, "Last 7 days" ),
                new (TimeIntervalType.Days, -30, "Last 30 days")
            ]
        },
        new() {
            Title = "Calendar months",
            TimeIntervals = [
                new (TimeIntervalType.Months, "This month"),
                new (TimeIntervalType.Quarters, "This quarter"),
                new (TimeIntervalType.Years, "This year"),
                new (TimeIntervalType.Months, -1, "Last month"),
                new (TimeIntervalType.Quarters, -1, "Last quarter"),
                new (TimeIntervalType.Months, -3, "Last 3 months"),
                new (TimeIntervalType.Months, -6, "Last 6 months"),
                new (TimeIntervalType.Months, -12, "Last 12 months"),
            ]
        }
    ];

    private TimeInterval? Selected { get; set; }

    /// <summary>
    /// When parameters are set, check if the PageQuery has a filter that matches our property by
    /// name. If not, create one and add it to the PageQuery.
    /// </summary>
    /// <remarks>Multiple filters mapped to the same PageQuery are not supported.</remarks>
    protected override void OnParametersSet()
    {
        if(QueryBuilderAccessor == null) {
            QueryBuilderAccessor = new QueryBuilderAccessor(Decorator);
            QueryBuilderAccessor.QueryBuilder.OnChanged += QueryBuilder_OnChanged;
        }
        Filter = QueryBuilderAccessor.QueryBuilder.Filters
            .FirstOrDefault(e => string.Equals(e.FilterName, Property.Property.Name, StringComparison.OrdinalIgnoreCase))
            as DateTimeFilterBuilder;
        if(Filter == null) {
            Filter = new DateTimeFilterBuilder { FilterName = Property.Property.Name };
            QueryBuilderAccessor.QueryBuilder.Filters.Add(Filter);
        }
        else {
            SyncValues();
        }
    }

    /// <summary>
    /// Conditional update this filter based on the query builder being changed.
    /// </summary>
    private void QueryBuilder_OnChanged(object? sender, EventArgs e)
    {
        if(filterInSync && Filter != null) {
            SyncValues();
        }
    }

    /// <summary>
    /// Show the MiniDialog.
    /// </summary>
    protected async Task DoButtonClick(MouseEventArgs args)
    {
        await MiniDialog!.ShowAsync();
    }

    private void OnIntervalChange(EventArgs _, TimeInterval timeInterval)
    {
        Selected = timeInterval;
        Filter?.Reset();
    }

    protected void DoPreviousClick()
    {
        if(Selected == null) { return; }

        if(!Selected.IsClone) {
            Selected = Selected.Clone();
        }
        Selected.Previous();
    }

    protected void DoNextClick()
    {
        if(Selected == null) { return; }

        if(!Selected.IsClone) {
            Selected = Selected.Clone();
        }
        Selected.Next();
    }

    protected void DoClearClick()
    {
        if(Selected == null) { return; }

        Selected = null;
        Filter?.Reset();
    }

    private Task SyncWithPageQuery()
    {
        if(Filter != null) {
            if(Selected == null) {
                Filter.Reset();
            }
            else {
                Logger.LogConsoleVerbose($"Applying {Selected.Summary} filter.");
                Selected.SetFilter(Filter);
            }

            filterInSync = false;
            QueryBuilderAccessor?.QueryBuilder.NotifyChanged();
            filterInSync = true;
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Synchronise the filter component values with the query builder.
    /// </summary>
    private void SyncValues()
    {
        if(Filter == null) { return; }

        var timeInterval = new TimeInterval();
        Selected = timeInterval.TryParseFilter(Filter) ? timeInterval : null;

        StateHasChanged();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if(QueryBuilderAccessor != null) {
            QueryBuilderAccessor.QueryBuilder.OnChanged -= QueryBuilder_OnChanged;
            QueryBuilderAccessor = null;
        }
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-field-filter", "filter-date", CssClass);

    private bool filterInSync = true;


    private struct TimeIntervalGroup
    {
        public string Title { get; set; }

        public List<TimeInterval> TimeIntervals { get; set; }
    }
}
