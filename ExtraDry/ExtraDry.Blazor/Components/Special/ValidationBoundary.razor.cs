namespace ExtraDry.Blazor;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "DRY1501:Blazor components should have a common properties.", Justification = "Inherited with DryErrorBoundary")]
public partial class ValidationBoundary : DryErrorBoundary
{
    [Parameter]
    public ValidationRenderMode MessageRenderMode { get; set; } = ValidationRenderMode.Before;

    protected Type ValidationMessageType => ErrorComponent ?? ThemeInfo?.ValidationMessageComponent ?? typeof(DryValidationSummary);
}
