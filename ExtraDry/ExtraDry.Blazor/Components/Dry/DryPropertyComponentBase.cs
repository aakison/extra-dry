namespace ExtraDry.Blazor.Components.Dry;

/// <summary>
/// Represents a set of common component functionality for the implementation
/// of a DRY component that reads and/or writes a property of a model object.
/// </summary>
public class DryPropertyComponentBase : ComponentBase {
    /// <summary>
    /// The link to the Model where values are stored.
    /// </summary>
    [Parameter, EditorRequired]
    public object Model { get; set; } = null!;

    /// <summary>
    /// The property description for the property.
    /// </summary>
    [Parameter]
    public PropertyDescription? Property { get; set; }

    /// <summary>
    /// If the PropertyDescription is not readily available, provide
    /// the property name (e.g. using nameof operator) and the description
    /// will be resolved.
    /// </summary>
    [Parameter]
    public string? PropertyName { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if(Model == null) {
            Logger.LogConsoleError("Model property must be supplied");
            return;
        }
        if(Property == null) {
            if(PropertyName == null) {
                Logger.LogConsoleError("A property must be specified using either Property or PropertyName.");
                return;
            }
            var propInfo = Model.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if(propInfo == null) {
                Logger.LogConsoleError($"The {PropertyName} did not specify a valid property on {Model.GetType()} in component {GetType()}");
                return;
            }
            Property = new PropertyDescription(propInfo);
        }
    }

    [Inject]
    protected ILogger<DryPropertyComponentBase> Logger { get; set; } = null!;

}

