namespace ExtraDry.Blazor;

public abstract class DryViewModelComponentBase : ComponentBase
{
    [Parameter, EditorRequired]
    public object ViewModel { get; set; } = null!;

    protected override void OnParametersSet()
    {
        if(ViewModel == null) {
            BaseLogger.LogConsoleError($"Component '{GetType().Name}' requires a ViewModel");
            return;
        }
        if(Description == null || Description.Decorator != ViewModel) {
            Description = new DecoratorInfo(ViewModel);
        }
    }

    protected DecoratorInfo? Description { get; set; }

    [Inject]
    private ILogger<DryViewModelComponentBase> BaseLogger { get; set; } = null!;
}
