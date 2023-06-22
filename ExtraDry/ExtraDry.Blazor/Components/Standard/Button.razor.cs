namespace ExtraDry.Blazor;

/// <summary>
/// A Button visually displays a button with structured content.  It raises OnClick events for
/// interacting with the clicking of the button.  This is a visual rendering component and is 
/// typically used from within `DryButton` and other components that render a button of any type.
/// </summary>
public partial class Button : ComponentBase, IExtraDryComponent {

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The key for the icon to be displayed on the button.  This icon will typically provide
    /// context for the button, such as "magnifying-glass" for a search button.
    /// </summary>
    [Parameter]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the context icon should be shown, independent of the content of the Icon 
    /// property.  Default: true.
    /// </summary>
    [Parameter]
    public bool ShowIcon { get; set; } = true;

    /// <summary>
    /// Indicates if the Caption should be shown on the button.  If not shown, the caption is still
    /// rendered as the title for the button for accessibility.  Default: true.
    /// </summary>
    [Parameter]
    public bool ShowCaption { get; set; } = true;

    /// <summary>
    /// The caption of the button. The caption is rendered both on the button and as the title for
    /// the button.  The caption may be optionally shown (use ShowCaption property), but is always
    /// rendered as the title to conform to accessibility guidelines.
    /// </summary>
    [Parameter, EditorRequired]
    public string Caption { get; set; } = null!;

    /// <summary>
    /// The key for an icon to indicate the UI functionality of the button.  This is a visual 
    /// indicator for the user providing UI expectations, such as "chevron-down" for a button that
    /// reveals a drop-down select list.
    /// </summary>
    [Parameter]
    public string Affordance { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the visual affordance icon should be shown, independent of the content of 
    /// the Affordance property.  Default: true.
    /// </summary>
    [Parameter]
    public bool ShowAffordance { get; set; } = true;

    /// <summary>
    /// For advanced rendering options, the button may also take any arbitrary child content.
    /// This content is rendered inside the button along with other content, use the "ShowXxx"
    /// properties to configure which elements are displayed.
    /// </summary>
    [Parameter] 
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Indicates if the child content for the button should be shown, independent of the content
    /// of whether child content was set or not.
    /// </summary>
    [Parameter]
    public bool ShowContent { get; set; } = true;

    /// <summary>
    /// Indicates if the button is currently enabled.  Note: this is the inverse of the HTML 
    /// disabled logic.
    /// </summary>
    [Parameter]
    public bool Enabled { get; set; } = true;

    [CascadingParameter]
    protected ThemeInfo? ThemeInfo { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass);

    private bool DisplayIcon => ShowIcon && !string.IsNullOrWhiteSpace(Icon);

    private bool DisplayCaption => ShowCaption && !string.IsNullOrWhiteSpace(Caption);

    private bool DisplayAffordance => ShowAffordance && !string.IsNullOrWhiteSpace(Affordance);

    private bool DisplayContent => ShowContent && ChildContent != null;

    private bool Disabled => !Enabled;

}
