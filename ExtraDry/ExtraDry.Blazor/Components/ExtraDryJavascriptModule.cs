#nullable enable

namespace ExtraDry.Blazor;

public class ExtraDryJavascriptModule : IAsyncDisposable {

    public ExtraDryJavascriptModule(IJSRuntime runtime)
    {
        Runtime = runtime;
    }

    public async ValueTask Initialize()
    {
        module ??= await Runtime.InvokeAsync<IJSObjectReference>("import", filename);
    }

    public async ValueTask InvokeVoidAsync(string name, params object?[]? args)
    {
        await Initialize();
        await module!.InvokeVoidAsync(name, args);
    }

    // As IJSObjectReference is IAsyncDisposable so should we, but it practice this is not called.
    // The underlying module appears to be cleared out and made null regardless, very odd.
    public async ValueTask DisposeAsync()
    {
        if(module is not null) {
            await module.DisposeAsync();
            module = null;
        }
    }

    private IJSRuntime Runtime { get; set; }

    private static IJSObjectReference? module;

    private const string filename = "./_content/ExtraDry.Blazor/bundles/extra-dry-blazor-module.min.js";
}

