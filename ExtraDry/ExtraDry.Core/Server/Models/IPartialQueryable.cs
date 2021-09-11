using ExtraDry.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExtraDry.Server {

    public interface IPartialQueryable<T> : IQueryable<T> {

        PagedCollection<T> ToPagedCollection();

        Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default);

        FilteredCollection<T> ToFilteredCollection();

        Task<FilteredCollection<T>> ToFilteredCollectionAsync(CancellationToken cancellationToken = default);

    }
}
