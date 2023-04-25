namespace ExtraDry.Server;

/// <summary>
/// Represents a Queryable that might only return a subset of the selected data.  The data may be
/// both filtered and/or paged when returning.  To get a Partial Queryable, use the 
/// `IQueryable.QueryWith` extension method.  The Partial Queryable then allows convenience methods
/// to return paged or filtered subsets, as well as general statistics.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPartialQueryable<T> : IQueryable<T> {

    /// <summary>
    /// Return a Paged Collection suitable for serialization that represents a single page of the
    /// total results along with a continuation token and additional page information.
    /// </summary>
    /// <returns>The page of the collection.</returns>
    PagedCollection<T> ToPagedCollection();

    /// <inheritdoc cref="ToPagedCollection" />
    Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default);

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
    /// returned are determined by the [Statistics] attributes on the entity's properties.
    /// </summary>
    /// <returns>The statistics.</returns>
    Statistics<T> ToStatistics();

    /// <inheritdoc cref="ToStatistics" />
    Task<Statistics<T>> ToStatisticsAsync(CancellationToken cancellationToken = default);

}
