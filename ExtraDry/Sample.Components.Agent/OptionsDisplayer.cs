using ExtraDry.Blazor;
using ExtraDry.Blazor.Internal;
using ExtraDry.Core;
using ExtraDry.Server.Agents;

namespace Sample.Components.Agent;

internal class OptionsDisplayer(
    ILogger<OptionsDisplayer> logger,
    IListClient<Customer> tenants)
    : ICronJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var result = await tenants.GetItemsAsync(new Query(), cancellationToken);
        logger.LogInformation("Tenant Count: {count}", result.Items.Count());
    }
}
