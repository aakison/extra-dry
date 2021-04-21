#nullable enable

using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {

    public interface IListService<T> : IOptionProvider<T> {

        string UriTemplate { get; set; }

        object[] UriArguments { get; set; }

        int FetchSize { get; set; }

        ValueTask<ItemsProviderResult<T>> GetItemsAsync(string? sort, bool? ascending, int? skip, int? take, CancellationToken cancellationToken = default);

    }
}
