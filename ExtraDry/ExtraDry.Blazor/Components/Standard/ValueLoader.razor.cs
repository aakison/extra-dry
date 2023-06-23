namespace ExtraDry.Blazor;
/// <summary>
/// Represents a value that requires an async operation to populate.
/// Can be in the following states:
/// - Loading
/// - Error
/// - Timeout
/// - Complete - Loaded with Value
/// </summary>
public partial class ValueLoader<ValueModel> : ComponentBase, IExtraDryComponent {
    /// <summary>
    /// Render Fragment for when the value has been loaded. This will govern how the value is displayed
    /// </summary>
    [Parameter]
    public RenderFragment<ValueLoaderContext>? Display { get; set; }
    /// <summary>
    /// Render Fragment for when an error is encountered during loading.
    /// A default is provided but this can be used to override the display
    /// </summary>
    [Parameter]
    public RenderFragment<ValueLoaderContext>? Error { get; set; }
    /// <summary>
    /// Render Fragment for when a timeout is encountered during loading.
    /// A default is provided but this can be used to override the display
    /// </summary>
    [Parameter]
    public RenderFragment<ValueLoaderContext>? Timeout { get; set; }
    /// <summary>
    /// Render Fragment for when a value is in the process of being loaded.
    /// A default is provided but this can be used to override the display
    /// </summary>
    [Parameter]
    public RenderFragment<ValueLoaderContext>? Loading { get; set; }
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
    /// <inheritdoc cref="SpinnerSize" />
    [Parameter]
    public SpinnerSize Size { get; set; } = SpinnerSize.Standard;

    /// <summary>
    /// The value once loaded
    /// </summary>
    public ValueModel? Value { get; set; }

    /// <inheritdoc cref="LoadingState" />
    public LoadingState State { get; set; }  

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, "value-loader", State.ToString().ToLower());

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
    public class ValueLoaderContext {
        /// <summary>
        /// The loaded data value
        /// </summary>
        public ValueModel? Value { get; set; }
        /// <summary>
        /// The size of the loading icon
        /// </summary>
        public SpinnerSize Size { get; set; }
        /// <summary>
        /// A callback method to retry the load process
        /// </summary>
        public Func<Task>? Reload { get; set; }
    }
}

/// <summary>
/// Defines the state the ValueLoader is in
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
