using System.Threading.Tasks;

namespace Blazor.ExtraDry {

    public interface IEntityResolver<T> {
        Task<T> ResolveAsync(T exemplar);
    }

}
