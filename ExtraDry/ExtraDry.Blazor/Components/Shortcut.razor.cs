using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// A non-visual component that provides global keyboard shortcut functionality. When the specified
/// key combination is pressed, it will invoke the OnClick callback. This component can be placed
/// anywhere in the component tree and will listen for global keyboard events.
/// </summary>
[SuppressMessage("Usage", "DRY1500:Extra DRY Blazor components should have an interface.", Justification = "Component has no UI")]
public partial class Shortcut : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Keyboard shortcut that will trigger the OnClick event. Format: "Ctrl+Shift+F" or
    /// "Alt+Enter", etc. Supports combinations of Ctrl, Shift, Alt, Meta/Cmd modifiers with any
    /// key.
    /// </summary>
    [Parameter, EditorRequired]
    public string Keys { get; set; } = null!;

    /// <summary>
    /// Event callback that is invoked when the keyboard shortcut is pressed.
    /// </summary>
    [Parameter, EditorRequired]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [JSInvokable]
    public async Task OnShortcutPressed()
    {
        if(OnClick.HasDelegate) {
            await OnClick.InvokeAsync(new MouseEventArgs());
        }
    }

    public async ValueTask DisposeAsync()
    {
        if(!string.IsNullOrWhiteSpace(Keys)) {
            await Javascript.InvokeVoidAsync("Shortcut_UnregisterShortcut", Keys);
        }

        dotNetRef?.Dispose();
        GC.SuppressFinalize(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender && !string.IsNullOrWhiteSpace(Keys)) {
            dotNetRef = DotNetObjectReference.Create(this);
            await Javascript.InvokeVoidAsync("Shortcut_RegisterShortcut", Keys, dotNetRef);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task OnParametersSetAsync()
    {
        // Handle key changes
        if(!string.IsNullOrWhiteSpace(previousKeys) && previousKeys != Keys) {
            await Javascript.InvokeVoidAsync("Shortcut_UnregisterShortcut", previousKeys);
        }

        if(!string.IsNullOrWhiteSpace(Keys) && Keys != previousKeys && dotNetRef != null) {
            await Javascript.InvokeVoidAsync("Shortcut_RegisterShortcut", Keys, dotNetRef);
        }

        previousKeys = Keys;
        await base.OnParametersSetAsync();
    }

    [Inject]
    private ExtraDryJavascriptModule Javascript { get; set; } = null!;

    private DotNetObjectReference<Shortcut>? dotNetRef;

    private string? previousKeys;
}
