using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

/// <summary>
/// Wrapper for ListService to IOptionService so that the list can be in Core (the options are Web/Blazor only).
/// </summary>
public class ListServiceOptionProvider<T>(
    ListClient<T> source)
    : IOptionProvider<T>
{
    public async ValueTask<ItemsProviderResult<T>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        var result = await source.GetItemsAsync(new Query(), cancellationToken);
        return new ItemsProviderResult<T>(result.Items, result.Count);
    }
}
