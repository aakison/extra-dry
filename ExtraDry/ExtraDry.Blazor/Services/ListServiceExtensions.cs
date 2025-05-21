using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraDry.Blazor;

public static class ListServiceExtensions
{

    public static async ValueTask<ItemsProviderResult<T>> GetItemsAsync<T>(this ListService<T> list, CancellationToken cancellationToken)
    {
        var query = new Query { Source = ListSource.List };
        return await GetItemsAsync(list, query, cancellationToken);
    }

    public static async ValueTask<ItemsProviderResult<T>> GetItemsAsync<T>(this ListService<T> list, Query query, CancellationToken cancellationToken = default)
    {
        var result = await list.GetItemsInternalAsync(query, cancellationToken);
        return new ItemsProviderResult<T>(result.Item2, result.Item3);
    }

}
