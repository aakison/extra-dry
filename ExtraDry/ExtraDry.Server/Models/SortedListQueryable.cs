﻿using System.Linq.Expressions;

namespace ExtraDry.Server;

public class SortedListQueryable<T> : FilteredListQueryable<T>
{
    protected SortedListQueryable()
    { }

    public SortedListQueryable(IQueryable<T> queryable, SortQuery sortQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        Query = sortQuery;
        FilteredQuery = ApplyKeywordFilter(queryable, sortQuery, defaultFilter);
        SortedQuery = ApplyPropertySort(FilteredQuery, sortQuery);
        PagedQuery = SortedQuery;
    }

    public SortedCollection<T> ToSortedCollection()
    {
        return CreateSortedCollection(SortedQuery.ToList());
    }

    public async Task<SortedCollection<T>> ToSortedCollectionAsync(CancellationToken cancellationToken = default)
    {
        return CreateSortedCollection(await ToListAsync(SortedQuery, cancellationToken));
    }

    private SortedCollection<T> CreateSortedCollection(IList<T> items)
    {
        return new() {
            Items = items,
            Filter = Query.Filter,
            Sort = (Query as SortQuery)?.Sort,
        };
    }

    protected IQueryable<T> ApplyPropertySort(IQueryable<T> queryable, SortQuery query)
    {
        return queryable.Sort(query);
    }
}
