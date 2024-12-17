namespace ExtraDry.Blazor;

/// <summary>
/// Additional abstraction of JavaScript modules for use by Extra DRY, use instead of IJSRuntime.
/// </summary>
public class ExtraDryJavascriptModule
{

    /// <summary>
    /// Constructor that expects runtime, for use with dependency injection.
    /// </summary>
    public ExtraDryJavascriptModule(IJSRuntime runtime)
    {
        Runtime = runtime;
        Version = GetType().Assembly.GetName()?.Version?.ToString() ?? "1.0";
    }

    /// <summary>
    /// Invoke a function inside the 'extra-dry-blazor-module' module.
    /// Usage is same as IJSRuntime, but method must be exposed by Extra DRY module.
    /// </summary>
    public async ValueTask InvokeVoidAsync(string name, params object?[]? args)
    {
        await using var module = await Runtime.InvokeAsync<IJSObjectReference>("import", Filename);
        await module.InvokeVoidAsync(name, args);
    }

    private IJSRuntime Runtime { get; set; }

    private string Filename => $"/_content/ExtraDry.Blazor/js/extra-dry-blazor-module.min.js?v={Version}";

    private string Version { get; set; }

}
