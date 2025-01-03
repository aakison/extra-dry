namespace GettingStarted;

using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Worker(IBus bus) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested) {
            await bus.Publish(new GenericEvent { Value = $"The time is {DateTimeOffset.Now}" }, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
