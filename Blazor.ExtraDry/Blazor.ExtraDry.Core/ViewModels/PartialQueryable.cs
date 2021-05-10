using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {

    public interface IPartialQueryable<T> : IQueryable<T> {

        PartialCollection<T> ToPartialCollection();
    
        Task<PartialCollection<T>> ToPartialCollectionAsync(CancellationToken cancellationToken = default);

    }

    public class PartialQueryable<T> : IPartialQueryable<T> {

        public PartialQueryable(IQueryable<T> queryable, PartialQuery partialQuery)
        {
            query = partialQuery;
            token = ContinuationToken.FromString(query.Token);
            filteredQuery = queryable.Filter(partialQuery);
            sortedQuery = filteredQuery.Sort(partialQuery);
            pagedQuery = sortedQuery.Page(partialQuery);
        }

        public Type ElementType => pagedQuery.ElementType;

        public Expression Expression => pagedQuery.Expression;

        public IQueryProvider Provider => pagedQuery.Provider;

        public IEnumerator GetEnumerator() => pagedQuery.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => pagedQuery.GetEnumerator();

        public PartialCollection<T> ToPartialCollection()
        {
            var items = pagedQuery.ToList();
            return CreatePartialCollection(items);
        }

        public async Task<PartialCollection<T>> ToPartialCollectionAsync(CancellationToken cancellationToken = default)
        {
            // Logic like EF Core `.ToListAsync` but without taking a dependency on that entire package.
            var items = new List<T>();
            if(pagedQuery is IAsyncEnumerable<T> pagedAsyncQuery) {
                await foreach(var element in pagedAsyncQuery.WithCancellation(cancellationToken)) {
                    items.Add(element);
                }
            }
            else {
                throw new InvalidOperationException("");
            }
            return CreatePartialCollection(items);
        }

        private PartialCollection<T> CreatePartialCollection(List<T> items)
        {
            var nextToken = new ContinuationToken(query.Filter, query.Sort, query.Ascending, "Id", query.Skip, query.Take, token);
            var previousTake = ContinuationToken.ActualTake(token, query.Take);
            var previousSkip = ContinuationToken.ActualSkip(token, query.Skip);
            var total = items.Count == previousTake ? filteredQuery.Count() : previousSkip + items.Count;
            return new PartialCollection<T>(items) {
                Filter = nextToken.Filter,
                Sort = nextToken.Sort,
                Start = previousSkip,
                Total = total,
                ContinuationToken = nextToken.ToString(),
            };
        }

        private readonly PartialQuery query;

        private readonly ContinuationToken token;

        private readonly IQueryable<T> filteredQuery;

        private readonly IQueryable<T> sortedQuery;

        private readonly IQueryable<T> pagedQuery;
    }
}
