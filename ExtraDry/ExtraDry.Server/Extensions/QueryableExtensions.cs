using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExtraDry.Server;

/// <summary>
/// A very lightweight dynamic linq builder, just enough to satisfy needs of filtering, sorting and
/// paging API result sets.
/// </summary>
public static class QueryableExtensions
{
    public static FilteredListQueryable<T> QueryWith<T>(this IQueryable<T> source, FilterQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class
    {
        return new FilteredListQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    /// <summary>
    /// Given a `FilterQuery`, dynamically constructs an expression query that applies the
    /// indicated filtering and sorting.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">A sort query that contains filtering and sorting information.</param>
    /// <param name="defaultFilter">
    /// An expression that provides default filtering support, which can be overridden by
    /// `filterQuery`.
    /// </param>
    public static SortedListQueryable<T> QueryWith<T>(this IQueryable<T> source, SortQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class
    {
        query.Stabilization = Options.Stabilization;
        return new SortedListQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    /// <summary>
    /// Given a `PageQuery`, dynamically constructs an expression query that applies the indicated
    /// filtering, sorting, and paging.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">
    /// A page query that contains filtering, sorting, and paging information.
    /// </param>
    /// <param name="defaultFilter">
    /// An expression that provides default filtering support, which can be overridden by
    /// `partialQuery`.
    /// </param>
    public static PagedListQueryable<T> QueryWith<T>(this IQueryable<T> source, PageQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class
    {
        query.Stabilization = Options.Stabilization;
        return new PagedListQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    /// <summary>
    /// Given a <see cref="HierarchyQuery" />, dynamically constructs an expression query that
    /// applies the indicated level and keyword filtering, expansions, and collapses.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">
    /// A hierarchy query that contains filtering, expansion, and collapse information.
    /// </param>
    /// <param name="defaultFilter">
    /// An expression that provides default filtering support, which can be overridden by
    /// `partialQuery`.
    /// </param>
    public static FilteredHierarchyQueryable<T> QueryWith<T>(this IQueryable<T> source, HierarchyQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class, IHierarchyEntity<T>
    {
        return new FilteredHierarchyQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    /// <summary>
    /// Given a <see cref="PageHierarchyQuery" />, dynamically constructs an expression query that
    /// applies the indicated level and keyword filtering, expansions, collapses, and paging.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">
    /// A page hierarchy query that contains filtering, expansion, collapse and paging information.
    /// </param>
    /// <param name="defaultFilter">
    /// An expression that provides default filtering support, which can be overridden by
    /// `partialQuery`.
    /// </param>
    public static PagedHierarchyQueryable<T> QueryWith<T>(this IQueryable<T> source, PageHierarchyQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class, IHierarchyEntity<T>
    {
        return new PagedHierarchyQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    public static BaseCollection<T> ToBaseCollection<T>(this IQueryable<T> source)
    {
        return new BaseCollection<T> {
            Items = source.ToList(),
        };
    }

    public static async Task<BaseCollection<T>> ToBaseCollectionAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
    {
        var baseQuery = new BaseQueryable<T>(source);
        return await baseQuery.ToBaseCollectionInternalAsync(cancellationToken);
    }

    internal static ExtraDryOptions Options { get; set; } = new ExtraDryOptions();

    /// <summary>
    /// The stabilization method to be used in queryable sorting.
    /// </summary>
    private static SortStabilization SortStabilization => Options.Stabilization;

    private const string MissingStabilizerErrorMessage = "Sort requires that a single EF key is uniquely defined to stabalize the sort, even if another sort property is present.  Use a single unique key following EF conventions";

    private const string SortErrorUserMessage = "Unable to Sort. 0x0F3F241C";
}
