namespace ExtraDry.Blazor;

/// <summary>
/// Represents a value that requires an async operation to populate.
/// Can be in the following states:
/// - Loading
/// - Error
/// - Timeout
/// - Complete - Loaded with Value
/// </summary>
public partial class Suspense<TModel> : ComponentBase, IExtraDryComponent {
    /// <summary>
    /// Render Fragment for when the value has been loaded. This will govern how the value is displayed
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<TModel?>? ChildContent { get; set; }

    /// <summary>
    /// Render Fragment for when an error is encountered during loading.
    /// A default is provided but this can be used to override the display
    /// </summary>
    [Parameter]
    public RenderFragment<IndicatorContext>? Error { get; set; }

    /// <summary>
    /// Render Fragment for when a timeout is encountered during loading.
    /// A default is provided but this can be used to override the display
    /// </summary>
    [Parameter]
    public RenderFragment<IndicatorContext>? Timeout { get; set; }

    /// <summary>
    /// Render Fragment for when a value is in the process of being loaded.
    /// A default is provided but this can be used to override the display
    /// </summary>
    [Parameter]
    public RenderFragment<IndicatorContext>? Fallback { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Delegate function for how to retrieve it's Value 
    /// Requires a method that is async and returns Task&lt;object?$gt;
    /// </summary>
    [Parameter, EditorRequired]    
    public SuspenseItemsProviderDelegate<TModel>? ItemProvider { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <inheritdoc cref="IndicatorSize" />
    [Parameter]
    public IndicatorSize Size { get; set; } = IndicatorSize.Standard;

    [Parameter]
    public IndicatorIcon ShowIcons { get; set; } = IndicatorIcon.All;

    /// <summary>
    /// The time span to wait before timing out in milliseconds. Default is 5 seconds
    /// </summary>
    [Parameter]
    public int TimeoutDuration { get; set; } = 5000;

    [CascadingParameter]
    protected ThemeInfo? ThemeInfo { get; set; }

    [Inject]
    private ILogger<Suspense<TModel>> Logger { get; set; } = null!;

    /// <summary>
    /// The value once loaded
    /// </summary>
    public TModel? Value { get; set; }

    /// <inheritdoc cref="LoadingState" />
    public LoadingState State { get; set; }  

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, "suspense", State.ToString().ToLower());

    protected async override Task OnInitializedAsync()
    {
        await DoLoadData();
    }

    private async Task DoLoadData()
    {
        State = LoadingState.Loading;

        if(ItemProvider == null) {
            State = LoadingState.Error;
            Console.WriteLine("Error: No ItemProvider method passed");
            return;
        }

        try {
            var tokenSource = new CancellationTokenSource(TimeoutDuration);
            
            Value = await ItemProvider(tokenSource.Token);
            State = LoadingState.Complete;
        }
        catch(TaskCanceledException tcex) {
            State = LoadingState.Timeout;
            Logger.LogWarning(tcex, "Timeout caught in Suspense component");
        }
        catch(TimeoutException tex) {
            State = LoadingState.Timeout;
            Logger.LogWarning(tex, "Timeout caught in Suspense component");
        }
        catch(Exception ex) {
            State = LoadingState.Error;
            Logger.LogError(ex, "Error caught in Suspense component");
        }
    }

    public async Task Refresh()
    {
        await DoLoadData();
    }
}

/// <summary>
/// Defines the state of the Suspense component
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LoadingState {
    /// <summary>
    /// Value is still loading
    /// </summary>
    Loading,
    /// <summary>
    /// An error has been encountered during loading
    /// </summary>
    Error,
    /// <summary>
    /// An timeout has been encountered during loading
    /// </summary>
    Timeout,
    /// <summary>
    /// The value has been loaded
    /// </summary>
    Complete
}

/// <summary>
/// A function that provides the item to the Suspense component
/// </summary>
/// <typeparam name="TItem">The type of the context the item being loaded.</typeparam>
/// <param name="cancellationToken">The <see cref="CancellationToken"/> for the consumer to make use of to handle cancellation events and timeouts.</param>
/// <returns>A <see cref="Task"/> whose result is of type <c>TItem</c> upon successful completion.</returns>
public delegate Task<TItem> SuspenseItemsProviderDelegate<TItem>(CancellationToken cancellationToken);
