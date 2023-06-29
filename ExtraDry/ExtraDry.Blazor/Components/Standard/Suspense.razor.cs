namespace ExtraDry.Blazor;
/// <summary>
/// Represents a value that requires an async operation to populate.
/// Can be in the following states:
/// - Loading
/// - Error
/// - Timeout
/// - Complete - Loaded with Value
/// </summary>
public partial class Suspense<ValueModel> : ComponentBase, IExtraDryComponent {
    /// <summary>
    /// Render Fragment for when the value has been loaded. This will govern how the value is displayed
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<SuspenseContext>? ChildContent { get; set; }

    /// <summary>
    /// Render Fragment for when an error is encountered during loading.
    /// A default is provided but this can be used to override the display
    /// </summary>
    [Parameter]
    public RenderFragment<IndicatorContext>? ErrorIndicator { get; set; }

    /// <summary>
    /// Render Fragment for when a timeout is encountered during loading.
    /// A default is provided but this can be used to override the display
    /// </summary>
    [Parameter]
    public RenderFragment<IndicatorContext>? TimeoutIndicator { get; set; }

    /// <summary>
    /// Render Fragment for when a value is in the process of being loaded.
    /// A default is provided but this can be used to override the display
    /// </summary>
    [Parameter]
    public RenderFragment<IndicatorContext>? LoadingIndicator { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Delegate function for how to retrieve it's Value 
    /// Requires a method that is async and returns Task&lt;object?$gt;
    /// </summary>
    [Parameter]    
    public Func<Task<ValueModel?>>? LoadData { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    /// <inheritdoc cref="IndicatorSize" />
    [Parameter]
    public IndicatorSize Size { get; set; } = IndicatorSize.Standard;

    [CascadingParameter]
    protected ThemeInfo? ThemeInfo { get; set; }

    /// <summary>
    /// The value once loaded
    /// </summary>
    public ValueModel? Value { get; set; }

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

        if(LoadData == null) {
            State = LoadingState.Error;
            Console.WriteLine("Error: No LoadData method passed");
            return;
        }

        try {
            Value = await LoadData();
            State = LoadingState.Complete;
        }
        catch(TimeoutException tex) {
            State = LoadingState.Timeout;
            Console.WriteLine(tex.ToString());
        }
        catch(TaskCanceledException tcex) {
            State = LoadingState.Timeout;
            Console.WriteLine(tcex.ToString());
        }
        catch(Exception ex) {
            State = LoadingState.Error;
            Console.WriteLine(ex.ToString());
        }
    }

    public async Task Refresh()
    {
        await DoLoadData();
    }

    /// <summary>
    /// Context passed through to the child components
    /// </summary>
    public class SuspenseContext: IndicatorContext {
        /// <summary>
        /// The loaded data value
        /// </summary>
        public ValueModel? Value { get; set; }
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
