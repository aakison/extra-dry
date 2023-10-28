using ExtraDry.Server.Internal;
using System.Linq.Expressions;

namespace ExtraDry.Server;

/// <inheritdoc cref="IFilteredQueryable{T}" />
public class FilteredHierarchyQueryable<T> : FilteredQueryable<T> where T : IHierarchyEntity<T> {

    public FilteredHierarchyQueryable(IQueryable<T> queryable, HierarchyQuery hierarchyQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as FilteredListQueryable<T>)?.ForceStringComparison;
        Query = hierarchyQuery;
        // Level is either the entire set if using text filter, or just a depth otherwise.
        var levelQuery = string.IsNullOrEmpty(hierarchyQuery.Filter)
            ? queryable.Where(e => e.Lineage.GetLevel() <= hierarchyQuery.Level)
            : queryable;
        // Then filter with common filter mechanism
        FilteredQuery = ApplyKeywordFilter(levelQuery, hierarchyQuery, defaultFilter);
        // Ensure expanded slugs and ancestors are included, while excluding collapsed.
        var tempQuery = ExpandCollapseHierarchy(queryable, FilteredQuery, hierarchyQuery);
        // Then sort it the only way that is allowed, breadth-first by lineage.
        SortedQuery = tempQuery.OrderBy(e => e.Lineage);
        // Finally, page it.
        PagedQuery = SortedQuery.Page(0, PageQuery.DefaultTake, null);
    }

    //public FilteredHierarchyQueryable(IQueryable<T> queryable, PageHierarchyQuery pageHierarchyQuery, Expression<Func<T, bool>>? defaultFilter)
    //{
    //    ForceStringComparison = (queryable as FilteredListQueryable<T>)?.ForceStringComparison;
    //    Query = pageHierarchyQuery;
    //    Token = ContinuationToken.FromString(pageHierarchyQuery.Token);
    //    FilteredQuery = InitializeMergedFilter(queryable, pageHierarchyQuery, defaultFilter);
    //    SortedQuery = FilteredQuery.Sort(pageHierarchyQuery);
    //    PagedQuery = SortedQuery.Page(pageHierarchyQuery);
    //}

    protected static IQueryable<T> ExpandCollapseHierarchy(IQueryable<T> baseQueryable, IQueryable<T> filteredQueryable, HierarchyQuery query)
    {
        var ancestors = AncestorOf(filteredQueryable);
        var expansions = ChildrenOf(query.Expand);
        var collapses = DescendantOf(query.Collapse);

        //var all = filtered.Union(ancestors);
        var all = filteredQueryable.Union(expansions).Union(ancestors).Except(collapses).OrderBy(e => e.Lineage);
        return all;

        IQueryable<T> ChildrenOf(IEnumerable<string> parentSlugs) =>
            baseQueryable.SelectMany(parent => baseQueryable
                .Where(child => child.Lineage.IsDescendantOf(parent.Lineage)
                            && child.Lineage.GetLevel() == parent.Lineage.GetLevel() + 1
                            && parentSlugs.Contains(parent.Slug)),
                (parent, child) => child);

        IQueryable<T> DescendantOf(IEnumerable<string> parentSlugs) =>
            baseQueryable.SelectMany(parent => baseQueryable
                .Where(child => child.Lineage.IsDescendantOf(parent.Lineage)
                    && child.Lineage.GetLevel() > parent.Lineage.GetLevel()
                    && parentSlugs.Contains(parent.Slug)),
                (parent, child) => child);

        IQueryable<T> AncestorOf(IQueryable<T> filteredSubset) =>
            baseQueryable.SelectMany(ancestor => filteredSubset
                .Where(descendant => descendant.Lineage.IsDescendantOf(ancestor.Lineage)
                    && descendant.Lineage.GetLevel() != ancestor.Lineage.GetLevel()),
                (ancestor, descendant) => ancestor);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollection"/>
    public override PagedCollection<T> ToPagedCollection()
    {
        var items = PagedQuery.ToList();
        return CreatePartialCollection(items);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public override async Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default)
    {
        // Logic like EF Core `.ToListAsync` but without taking a dependency on that package into Blazor.
        var items = new List<T>();
        if(PagedQuery is IAsyncEnumerable<T> pagedAsyncQuery) {
            await foreach(var element in pagedAsyncQuery.WithCancellation(cancellationToken)) {
                items.Add(element);
            }
        }
        else {
            items.AddRange(PagedQuery);
        }
        return CreatePartialCollection(items);
    }

    private PagedCollection<T> CreatePartialCollection(List<T> items)
    {
        var skip = (Query as PageQuery)?.Skip ?? 0;
        var take = (Query as PageQuery)?.Take ?? PageQuery.DefaultTake;

        var nextToken = (Token ?? new ContinuationToken(Query.Filter, Sort, take, take)).Next(skip, take);
        var previousTake = ContinuationToken.ActualTake(Token, take);
        var previousSkip = ContinuationToken.ActualSkip(Token, skip);
        var total = items.Count == previousTake ? FilteredQuery.Count() : previousSkip + items.Count;
        return new PagedCollection<T> {
            Items = items,
            Filter = nextToken.Filter,
            Sort = nextToken.Sort,
            Start = previousSkip,
            Total = total,
            ContinuationToken = nextToken.ToString(),
        };
    }

    protected override string? Sort => nameof(IHierarchyEntity<T>.Lineage).ToLowerInvariant();

}
