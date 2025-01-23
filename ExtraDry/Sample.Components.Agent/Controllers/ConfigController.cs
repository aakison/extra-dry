using GettingStarted.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Sample.Components.Agent.Controllers;

public class ConfigController(
    IBus bus)
    : Controller
{
    [HttpGet("/config")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Pattern for controllers is instance methods.")]
    public async Task<string> RetrieveConfig()
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
