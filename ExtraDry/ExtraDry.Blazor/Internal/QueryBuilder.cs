using System.ComponentModel;

namespace ExtraDry.Blazor.Internal;

/// <summary>
/// Builds a PageQuery for posting back to a server using a set of filters.  This also has change
/// notifications so that display components can sync to the changing filters.  This is the primary
/// mechanism by which filter components inform table views of filters and required refreshes.
/// </summary>
public class QueryBuilder {

    /// <inheritdoc cref="QueryBuilder" />
    public QueryBuilder() { 
        TextFilter = new TextFilterBuilder() { FilterName = "Keywords" };
        Filters.Add(TextFilter);
    }

    /// <summary>
    /// Event to subscribe to to be notified when the page query has changed and views should 
    /// be refreshed.
    /// </summary>
    public event EventHandler? OnChanged;

    /// <summary>
    /// Manually rebuilds the query and notifices all observers that changes have been made.
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
            Level = Level,
            Expand = Expand.ToArray(),
            Collapse = Collapse.ToArray(),
            Source = Source,
        };
        return Query;
    }

    private string BuildFilter() => string.Join(' ', Filters.Select(e => e.Build()).Where(e => !string.IsNullOrWhiteSpace(e))).Trim();

    private string BuildSort() => Sort.Build();

    public void Reset()
    {
        foreach(var filter in Filters) {
            filter.Reset();
        }
        NotifyChanged();
    }

    /// <summary>
    /// A list of all filterable items that this page query supports.  These supports any filter
    /// concept that can be bound to such as free-text, enum select lists, etc.
    /// </summary>
    public List<FilterBuilder> Filters { get; } = new();

    /// <summary>
    /// A generic text filter that can be applied.  This is typically just words written by users
    /// but will technically support any ExtraDry FilterQuery.  
    /// </summary>
    public TextFilterBuilder TextFilter { get; }

    /// <summary>
    /// The currently active page query.  Filters will be updated and will notify through OnChanged 
    /// when a new Query is available.
    /// </summary>
    public Query Query { get; private set; } = new();

    public ListSource Source { get; set; } = ListSource.Hierarchy;

    public int Level {
        get => level;
        set {
            if(value != level) {
                level = value;
                NotifyChanged();
            }
        }
    }
    private int level = 100;

    public List<string> Expand { get; } = new();

    public List<string> Collapse { get; } = new();

    public SortBuilder Sort { get; } = new();

    public int? Skip { get; set; }

}

public class Query {

    public ListSource Source { get; init; }

    public string? Filter { get; init; }

    public string? Sort { get; init; }

    public int? Skip { get; init; }

    public int? Take { get; init; }

    public int? Level { get; init; }

    public string[]? Expand { get; init; }

    public string[]? Collapse { get; init; }

}

public enum ListSource
{
    List,
    Hierarchy,
}
