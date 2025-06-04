using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

/// <summary>
/// Similar to IListService but used for providing options to the Virtulized components only available in Blazor.
/// </summary>
public interface IOptionProvider<T>
{
    ValueTask<ItemsProviderResult<T>> GetItemsAsync(CancellationToken cancellationToken = default);
}
