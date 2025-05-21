namespace ExtraDry.Blazor;

/// <summary>
/// Wrapper for ListService to IOptionService so that the list can be in Core (the options are Web/Blazor only).
/// </summary>
public class ListServiceOptionProvider<T>(
    ListService<T> source)
    : IOptionProvider<T>
{
    public async ValueTask<Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult<T>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        return await source.GetItemsAsync(cancellationToken);
    }
}
