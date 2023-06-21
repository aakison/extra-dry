using ExtraDry.Core;

namespace ExtraDry.Blazor;
/// <summary>
/// Represents a value that requires an async operation to populate.
/// Can be in the following states:
/// - Loading
/// - Error
/// - Timeout
/// - Complete - Loaded with Value
/// </summary>
public partial class ValueLoader : ComponentBase, IExtraDryComponent {
    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    /// <summary>
    /// Delegate function for how to retrieve it's Value 
    /// Requires a method that is async and returns Task&lt;object?$gt;
    /// </summary>
    public Func<Task<object?>>? LoadData { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    /// <summary>
    /// The value once loaded
    /// </summary>
    public object? Value { get; set; }

    public LoadingState State { get; set; }

    public string StateMessage { get; set; } = string.Empty;    

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, "value-loader");

    protected async override Task OnInitializedAsync()
    {
        await DoLoadData();
    }

    private async Task DoLoadData()
    {
        State = LoadingState.Loading;

        if(LoadData == null) {
            State = LoadingState.Error;
            StateMessage = "No data to load";
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
            StateMessage = "An error has occurred";
            Console.WriteLine(ex.ToString());
        }
    }

    public async Task Refresh()
    {
        await DoLoadData();
    }
}

public enum LoadingState {
    Loading,
    Error,
    Timeout,
    Complete
}
