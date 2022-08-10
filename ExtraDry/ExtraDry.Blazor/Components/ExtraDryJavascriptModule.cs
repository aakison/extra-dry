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

    public async ValueTask DisposeAsync()
    {
        Console.WriteLine("disposing module");
        //if(module is not null) {
        //    await module.DisposeAsync();
        //    module = null;
        //}
    }

    private IJSRuntime Runtime { get; set; }

    private static IJSObjectReference? module;

    private const string filename = "./_content/ExtraDry.Blazor/bundles/extra-dry-blazor-module.min.js";
}

