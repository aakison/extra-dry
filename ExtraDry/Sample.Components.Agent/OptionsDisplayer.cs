﻿using ExtraDry.Blazor;
using ExtraDry.Server.Agents;

namespace Sample.Components.Agent;

internal class OptionsDisplayer(
    ILogger<OptionsDisplayer> logger,
    IListService<Tenant> tenants)
    : ICronJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var result = await tenants.GetItemsAsync(new Query(), cancellationToken);
        logger.LogInformation("Tenant Count: {count}", result.Items.Count());
    }

}