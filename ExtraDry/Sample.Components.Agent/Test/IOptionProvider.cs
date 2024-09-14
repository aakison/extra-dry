using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

public interface IOptionProvider<T> {

    ValueTask<ItemsProviderResult<T>> GetItemsAsync(CancellationToken cancellationToken = default);

}
