﻿using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class BaseQueryable<T> : IQueryable<T>
{
    protected BaseQueryable() { }

    public BaseQueryable(IQueryable<T> queryable, StringComparison? forceStringComparison = null)
    {
        ForceStringComparison = forceStringComparison;
        Query = new();
        PagedQuery = SortedQuery = FilteredQuery = queryable;
    }

    internal ContinuationToken? Token { get; set; }

    protected FilterQuery Query { get; set; } = null!;

    protected IQueryable<T> FilteredQuery { get; set; } = null!;

    protected IQueryable<T> SortedQuery { get; set; } = null!;

    protected IQueryable<T> PagedQuery { get; set; } = null!;

    public Type ElementType => PagedQuery.ElementType;

    public Expression Expression => PagedQuery.Expression;

    public IQueryProvider Provider => PagedQuery.Provider;

    public IEnumerator GetEnumerator() => PagedQuery.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => PagedQuery.GetEnumerator();

    /// <summary>
    /// Helper to perform either a ToSingle or ToSingleAsync depending on the type of queryable.
    /// </summary>
    protected async Task<TItem> ToSingleAsync<TItem>(IQueryable<TItem> queryable, CancellationToken cancellationToken = default)
    {
        if(queryable is IAsyncEnumerable<TItem>) {
            // version for EF to database.
            return await queryable.SingleAsync(cancellationToken);
        }
        else {
            // version for in-memory querables, including unit tests.
            return queryable.Single();
        }
    }

    /// <summary>
    /// Helper to perform either a ToList or ToListAsync depending on the type of queryable.
    /// </summary>
    protected async Task<List<TItem>> ToListAsync<TItem>(IQueryable<TItem> queryable, CancellationToken cancellationToken = default)
    {
        if(queryable is IAsyncEnumerable<TItem>) {
            // version for EF to database.
            return await queryable.ToListAsync(cancellationToken);
        }
        else {
            // version for in-memory querables, including unit tests.
            return queryable.ToList();
        }
    }

    /// <summary>
    /// Helper to perform either a ToCount or ToCountAsync depending on the type of queryable.
    /// </summary>
    protected async Task<int> ToCountAsync<TItem>(IQueryable<TItem> queryable, CancellationToken cancellationToken = default)
    {
        if(queryable is IAsyncQueryProvider) {
            // version for EF to database.
            return await queryable.CountAsync(cancellationToken);
        }
        else {
            // version for in-memory querables, including unit tests.
            return queryable.Count();
        }
    }

    internal async Task<BaseCollection<T>> ToBaseCollectionInternalAsync(CancellationToken cancellationToken = default) =>
        BaseQueryable<T>.CreateBaseCollection(await ToListAsync(FilteredQuery, cancellationToken));

    private static BaseCollection<T> CreateBaseCollection(List<T> items) =>
        new() {
            Items = items,
        };

    /// <summary>
    /// Used for in-memory databases that are case sensitive when doing instructions page.  Not
    /// useful in general and can actually cause confusion.  Consider a different mechanism that 
    /// doesn't raise this up to the end-developer level.  
    /// </summary>
    public StringComparison? ForceStringComparison { get; protected set; }

}