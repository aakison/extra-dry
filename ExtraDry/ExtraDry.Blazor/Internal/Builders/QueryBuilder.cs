namespace ExtraDry.Blazor.Internal;

/// <summary>
/// Builds a PageQuery for posting back to a server using a set of filters. This also has change
/// notifications so that display components can sync to the changing filters. This is the primary
/// mechanism by which filter components inform table views of filters and required refreshes.
/// </summary>
public class QueryBuilder
{
    /// <inheritdoc cref="QueryBuilder" />
    public QueryBuilder()
    {
        TextFilter = new TextFilterBuilder() { FilterName = "Keywords" };
        Filters.Add(TextFilter);
    }

    /// <summary>
    /// Event to subscribe to be notified when the page query has changed and views should be
    /// refreshed.
    /// </summary>
    public event EventHandler? OnChanged;

    /// <summary>
    /// Manually rebuilds the query and notifies all observers that changes have been made.
    /// </summary>
    public void NotifyChanged()
    {
        Query = Build();
        OnChanged?.Invoke(this, EventArgs.Empty);
    }

    public Query Build()
    {
        Query = new Query() {
            Filter = BuildFilter(),
            Sort = BuildSort(),
            Skip = Skip,
            Take = 50, // TODO: Make this configurable
            Level = BuildLevel(),
            Expand = Hierarchy.ExpandNodes.ToArray(),
            Collapse = Hierarchy.CollapseNodes.ToArray(),
            Source = Source,
        };
        return Query;
    }

    private string BuildFilter()
    {
        return string.Join(' ', Filters.Select(e => e.Build()).Where(e => !string.IsNullOrWhiteSpace(e))).Trim();
    }

    private string BuildSort()
    {
        return Sort.Build();
    }

    private int? BuildLevel()
    {
        return Level.Build();
    }

    public void Reset()
    {
        Hierarchy.Reset();
        Level.Reset();
        foreach(var filter in Filters) {
            filter.Reset();
        }
        NotifyChanged();
    }

    /// <summary>
    /// A list of all filterable items that this page query supports. These supports any filter
    /// concept that can be bound to such as free-text, enum select lists, etc.
    /// </summary>
    public List<FilterBuilder> Filters { get; } = [];

    /// <summary>
    /// A generic text filter that can be applied. This is typically just words written by users
    /// but will technically support any ExtraDry FilterQuery.
    /// </summary>
    public TextFilterBuilder TextFilter { get; }

    /// <summary>
    /// The currently active page query. Filters will be updated and will notify through OnChanged
    /// when a new Query is available.
    /// </summary>
    public Query Query { get; private set; } = new();

    public ListSource Source { get; set; } = ListSource.Hierarchy;

    public LevelBuilder Level { get; } = new();

    public HierarchyBuilder Hierarchy { get; } = new();

    public SortBuilder Sort { get; } = new();

    public int? Skip { get; set; }

    public void ParseFilters(string[] filters)
    {
        var notifyChanged = false;
        foreach(var filter in filters) {
            var filterKeyValue = filter.Split(':');
            if(filterKeyValue.Length != 2) { continue; }

            var propertyName = filterKeyValue[0];
            var filterValue = filterKeyValue[1];
            if(string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filterValue)) { continue; }

            var queryFilter = Filters.FirstOrDefault(f => f.FilterName == propertyName);
            switch(queryFilter) {
                case EnumFilterBuilder enumFilter:
                    if(UpdateEnumFilter(enumFilter, filterValue)) {
                        notifyChanged = true;
                    }
                    break;

                case DateTimeFilterBuilder dateTimeFilterBuilder:
                    if(dateTimeFilterBuilder.TryParseFilter(filter)) {
                        notifyChanged = true;
                    };
                    break;
            }
        }
        if(notifyChanged) {
            NotifyChanged();
        }
    }

    private static bool UpdateEnumFilter(EnumFilterBuilder enumFilter, string queryValue)
    {
        if(string.IsNullOrEmpty(queryValue)) { return false; }

        var values = queryValue.Split('|');
        foreach(var value in values) {
            enumFilter.Values.Add(value);
        }

        return true;
    }
}
