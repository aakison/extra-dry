using GettingStarted.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Components.Agent.Controllers;

public class ConfigController(
    IBus bus)
    : Controller
{
    [HttpGet("/config")]
    public async Task<string> RetrievConfig()
    {
        return await Task.FromResult("the config");
    }

    [HttpPost("/post-message")]
    public async Task<string> PostMessage([FromBody] GenericEvent message)
    {
        await bus.Publish(message);
        return message.Value;
    }
}
