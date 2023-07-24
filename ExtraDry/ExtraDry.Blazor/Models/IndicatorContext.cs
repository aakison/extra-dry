namespace ExtraDry.Blazor.Models;

/// <summary>
/// Context passed through to the indicator child components
/// </summary>
public class IndicatorContext {

    /// <summary>
    /// The size of the indicator icon
    /// </summary>
    public IndicatorSize Size { get; set; }

    /// <summary>
    /// A callback method to retry the load process
    /// </summary>
    public Func<Task> Reload { get; set; } = () => Task.CompletedTask;
}
