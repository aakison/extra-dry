namespace ExtraDry.Blazor;

/// <summary>
/// Options for configuring a SunEditor instance via JavaScript interop.
/// </summary>
public class MarkdownEditorOptions
{
    /// <summary>
    /// The toolbar mode: "Character" for inline controls, "Block" for full block-level controls.
    /// </summary>
    public string Mode { get; set; } = "Block";

    /// <summary>
    /// Whether to include the image button in the toolbar (Block mode only).
    /// </summary>
    public bool EnableImage { get; set; }

    /// <summary>
    /// Placeholder text displayed when the editor is empty.
    /// </summary>
    public string Placeholder { get; set; } = "";
}
