namespace ExtraDry.Server;

public interface IHierarchyQueryable<T> : IFilteredQueryable<T>
{
    HierarchyCollection<T> ToHierarchyCollection();

    Task<HierarchyCollection<T>> ToHierarchyCollectionAsync(CancellationToken cancellationToken = default);
}

//public interface IPagedHierarchyQueryable<T> : IHierarchyQueryable<T>
//{
//    PagedHierarchyCollection<T> ToPagedHierarchyCollection();

//    Task<PagedHierarchyCollection<T>> ToPagedHierarchyCollectionAsync(CancellationToken cancellationToken = default);
//}

public interface ISortedQueryable<T> : IFilteredQueryable<T>
{
    /// <summary>
    /// Return a Paged Collection suitable for serialization that represents a single page of the
    /// total results along with a continuation token and additional page information.
    /// </summary>
    /// <returns>The page of the collection.</returns>
    SortedCollection<T> ToSortedCollection();

    /// <inheritdoc cref="ToPagedCollection" />
    Task<SortedCollection<T>> ToSortedCollectionAsync(CancellationToken cancellationToken = default);

}

public interface IPagedQueryable<T> : ISortedQueryable<T>
{
    /// <summary>
    /// Return a Paged Collection suitable for serialization that represents a single page of the
    /// total results along with a continuation token and additional page information.
    /// </summary>
    /// <returns>The page of the collection.</returns>
    PagedCollection<T> ToPagedCollection();

    /// <inheritdoc cref="ToPagedCollection" />
    Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default);

}

/// <summary>
/// Represents a Queryable that might only return a subset of the selected data.  The data may be
/// both filtered and/or paged when returning.  To get a Partial Queryable, use the 
/// `IQueryable.QueryWith` extension method.  The Partial Queryable then allows convenience methods
/// to return paged or filtered subsets, as well as general statistics.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IFilteredQueryable<T> : IQueryable<T>
{

    /// <summary>
    /// Return a Filtered Collection suitable for serialization that represents a filtered subset
    /// of the total results along with information on the filter condition.
    /// </summary>
    /// <returns>The filtered subset collection.</returns>
    FilteredCollection<T> ToFilteredCollection();

    /// <inheritdoc cref="ToFilteredCollection" />
    Task<FilteredCollection<T>> ToFilteredCollectionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a set of statistics about an entity after a filter has been applied.  Statistics
    /// returned are determined by the <see cref="StatisticsAttribute" /> on the entity's properties.
    /// </summary>
    /// <returns>The statistics.</returns>
    Statistics<T> ToStatistics();

    /// <inheritdoc cref="ToStatistics" />
    Task<Statistics<T>> ToStatisticsAsync(CancellationToken cancellationToken = default);

}
