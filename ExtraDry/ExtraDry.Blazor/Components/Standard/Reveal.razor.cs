namespace ExtraDry.Blazor;

/// <summary>
/// A Blazor component that conditionally displays child content using a state system and CSS 
/// classes to enable common UI mechanisms such as expand/collapse, and fade-in/fade-out.
/// </summary>
public partial class Reveal : ComponentBase, IExtraDryComponent {
    
    /// <summary>
    /// The child content to be displayed.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The duration, in milliseconds, that the animation takes between revealed and 
    /// concealed states.
    /// </summary>
    [Parameter]
    public int Duration { get; set; } = 0;

    /// <summary>
    /// When the mode is `Expanding`, the height of the expanded content in pixels.  
    /// Ignored otherwise.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 50;

    /// <summary>
    /// Indicates if the child content is retained in the DOM or unloaded after being concealed.
    /// </summary>
    [Parameter]
    public bool KeepInDom { get; set; } = false;

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The mode use for the Reveal, can be either Expand or Fade for built-in effects, or CssOnly
    /// to enable custom reveal animations with your own CSS.
    /// </summary>
    [Parameter]
    public RevealMode Mode { get; set; } = RevealMode.Fade;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <summary>
    /// Reveals the components children as necessary.  Will start in Concealed state, progress
    /// through Revealing state for Duration milliseconds, then stop ate Revealed state.
    /// </summary>
    public async Task RevealAsync()
    {
        if(State == RevealState.None) {
            // UI will load content at this point.
            State = RevealState.Concealed;
            StateHasChanged();
            await Task.Delay(minimumDuration);
        }
        if(State == RevealState.Concealed) {
            State = RevealState.Revealing;
            StateHasChanged();
            await Task.Delay(AdjustedDuration);
            State = RevealState.Revealed;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Conceals the components children as necessary.  When starting from Revealed state, will
    /// progress to Concealing state for Duration milliseconds and then will reach Concealed state.
    /// If KeepInDom is false (the default), then the concealed state will occur only briefly as
    /// the content is unloaded from the DOM and the reveal state is set to None.
    /// </summary>
    /// <returns></returns>
    public async Task ConcealAsync()
    {
        if(State == RevealState.Revealed) {
            State = RevealState.Concealing;
            StateHasChanged();
            await Task.Delay(AdjustedDuration);
            State = RevealState.Concealed;
            StateHasChanged();
        }
        if(!KeepInDom && State == RevealState.Concealed) {
            await Task.Delay(minimumDuration);
            State = RevealState.None;
            StateHasChanged();
        }
    }

    public async Task ToggleAsync()
    {
        if(State == RevealState.Revealed) {
            await ConcealAsync();
        }
        else {
            await RevealAsync();
        }
    }

    /// <summary>
    /// The state of the reveal (e.g. expanding or showing).  Ties into the styles on the component
    /// to perform CSS based animations.
    /// None -> Concealed -> Revealing -> Revealed -> Concealing -\
    ///           ^-----------------------------------------------/
    /// </summary>
    public RevealState State { get; set; } = RevealState.None;

    public bool IsShown => State == RevealState.Revealed || State == RevealState.Concealing || State == RevealState.Revealing;

    private const int minimumDuration = 15; // One frame to allow refresh to happen.

    private int AdjustedDuration => Math.Clamp(Duration, minimumDuration, 3000);

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, "reveal", StateClass);

    private bool ShouldRenderChild => Mode == RevealMode.Fade || KeepInDom || State != RevealState.None;

    private string StateClass => State switch {
        RevealState.Revealing => "revealing",
        RevealState.Concealing => "concealing",
        RevealState.Revealed => "revealed",
        _ => "concealed",
    };

    private string InlineStyle {
        get {
            var style = string.Empty;
            if(Mode == RevealMode.Fade) {
                style = $"transition: opacity {AdjustedDuration / 1000f}s;";
                if(State == RevealState.Concealing || State == RevealState.Concealed || State == RevealState.None) {
                    style = $"{style} opacity: 0%;";
                }
                else {
                    style = $"{style} opacity: 100%;";
                }
            }
            else if(Mode == RevealMode.Expand) {
                style = $"transition: height {AdjustedDuration / 1000f}s; overflow: hidden;";
                if(State == RevealState.Concealing || State == RevealState.Concealed || State == RevealState.None) {
                    style = $"{style} height: 0;";
                }
                else {
                    style = $"{style} height: {Height}px";
                }
            }
            return style;
        }
    }
}
