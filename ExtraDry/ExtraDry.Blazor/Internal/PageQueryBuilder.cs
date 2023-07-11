namespace ExtraDry.Blazor.Internal;

/// <summary>
/// Builds a PageQuery for posting back to a server using a set of filters.  This also has change
/// notifications so that display components can sync to the changing filters.  This is the primary
/// mechanism by which filter components inform table views of filters and required refreshes.
/// </summary>
public class PageQueryBuilder {

    /// <inheritdoc cref="PageQueryBuilder" />
    public PageQueryBuilder() { 
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


    public PageQuery Build()
    {
        Query = new PageQuery() {
            Filter = string.Join(' ', Filters.Select(e => e.Build()).Where(e => !string.IsNullOrWhiteSpace(e))).Trim(),
        };
        return Query;
    }

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
    public PageQuery Query { get; private set; } = new();

}
