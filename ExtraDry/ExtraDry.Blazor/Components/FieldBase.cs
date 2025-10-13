namespace ExtraDry.Blazor.Components;

public abstract class FieldBase<T> : ComponentBase
{
    [Parameter]
    public T Value { get; set; } = default!;

    [Parameter]
    public EventCallback<T> ValueChanged { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnInput { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = "";

    /// <inheritdoc />
    [Parameter]
    public string Icon { get; set; } = "";

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    [Parameter]
    public bool ShowIcon { get; set; } = true;

    [Parameter]
    public string Affordance { get; set; } = "";

    [Parameter]
    public bool ShowAffordance { get; set; } = true;

    [Parameter]
    public string Placeholder { get; set; } = "";


    [Parameter] 
    public bool ShowPlaceholder { get; set; } = true;

    [Parameter]
    public string Id { get; set; } = "";


    /// <inheritdoc />
    [Parameter]
    public PropertySize Size { get; set; } = PropertySize.Medium;

    /// <inheritdoc />
    [Parameter]
    public string Description { get; set; } = "";

    /// <inheritdoc />
    [Parameter]
    public bool ShowDescription { get; set; } = true;

    /// <inheritdoc />
    [Parameter]
    public string Label { get; set; } = "";

    /// <inheritdoc />
    [Parameter]
    public bool ShowLabel { get; set; } = true;

    protected bool DisplayIcon => ShowIcon && Icon != "";

    protected bool DisplayAffordance => ShowAffordance && Affordance != "" && !ReadOnly;

    protected string InputId { get; set; } = "";

    protected override void OnInitialized()
    {
        InputId = Id switch {
            "" => $"{this.GetType().Name}{++instanceCount}",
            _ => Id,
        };
    }

    protected async Task NotifyChange(ChangeEventArgs args)
    {
        if(args.Value != null) {
            Value = (T)args.Value;
        }
        await ValueChanged.InvokeAsync(Value);
        await OnChange.InvokeAsync(args);
        //Validate();
    }

    protected async Task NotifyInput(ChangeEventArgs args)
    {
        if(args.Value != null) {
            Value = (T)args.Value;
        }
        await ValueChanged.InvokeAsync(Value);
        await OnInput.InvokeAsync(args);
    }

    private static int instanceCount;

}
