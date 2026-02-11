namespace ExtraDry.Blazor;

/// <summary>
/// Provides JavaScript interop for SunEditor markdown editing instances.
/// Manages the lifecycle of SunEditor instances tied to DOM elements and
/// communicates content changes back to Blazor via .NET object references.
/// </summary>
public sealed class MarkdownEditorInterop(IJSRuntime jsRuntime) : IAsyncDisposable
{

    /// <summary>
    /// Initializes a SunEditor instance on the specified DOM element.
    /// </summary>
    public async ValueTask InitializeAsync<T>(string elementId, DotNetObjectReference<T> dotNetRef, MarkdownEditorOptions options) where T : class
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("initialize", elementId, dotNetRef, options);
    }

    /// <summary>
    /// Retrieves the current HTML content from the SunEditor instance.
    /// </summary>
    public async ValueTask<string> GetContentAsync(string elementId)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<string>("getContent", elementId);
    }

    /// <summary>
    /// Sets HTML content into the SunEditor instance.
    /// </summary>
    public async ValueTask SetContentAsync(string elementId, string html)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("setContent", elementId, html);
    }

    /// <summary>
    /// Destroys the SunEditor instance and cleans up resources.
    /// </summary>
    public async ValueTask DestroyAsync(string elementId)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("destroy", elementId);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if(moduleTask.IsValueCreated) {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ExtraDry.Blazor/js/markdown-editor.js").AsTask());
}
