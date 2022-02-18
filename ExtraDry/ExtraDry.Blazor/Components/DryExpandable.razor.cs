#nullable enable

using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace ExtraDry.Blazor;

public partial class DryExpandable : ComponentBase {
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The duration, in milliseconds, that the animation takes between expanded and collapsed.
    /// </summary>
    [Parameter]
    public int Duration { get; set; } = 500;

    /// <summary>
    /// The height of the expanded area, in pixels.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 40;

    public async Task Expand()
    {
        if(State == ExpandedState.None) {
            // UI will load content at this point.
            State = ExpandedState.Collapsed;
            StateHasChanged();
            await Task.Delay(minimumDuration);
        }
        if(State == ExpandedState.Collapsed) {
            State = ExpandedState.Expanding;
            StateHasChanged();
            await Task.Delay(AdjustedDuration);
            State = ExpandedState.Expanded;
            StateHasChanged();
        }
    }

    public async Task Collapse()
    {
        if(State == ExpandedState.Expanded) {
            State = ExpandedState.Collapsing;
            StateHasChanged();
            await Task.Delay(AdjustedDuration);
            State = ExpandedState.Collapsed;
            StateHasChanged();
        }
    }

    public async Task Toggle()
    {
        if(State == ExpandedState.Expanded) {
            await Collapse();
        }
        else {
            await Expand();
        }
    }

    /// <summary>
    /// The state of the expanding and collapsing control, typical goes:
    /// None -> Collapsed -> Expanding -> Expanded -> Collapsing -\
    ///           ^-----------------------------------------------/
    /// </summary>
    public ExpandedState State { get; set; } = ExpandedState.None;

    private const int minimumDuration = 15; // One frame to allow refresh to happen.

    private int AdjustedDuration => Math.Clamp(Duration, minimumDuration, 3000);

    private string InlineStyle => $"transition: height {AdjustedDuration / 1000.0}s; height: {HeightString}";

    private string HeightString => State switch {
        ExpandedState.None => "0",
        ExpandedState.Collapsed => "0",
        ExpandedState.Collapsing => "0",
        _ => $"{Height}px",
    };

    private string CssClassState => $"expandable {State.ToString().ToLowerInvariant()}";

    public enum ExpandedState {
        /// <summary>
        /// The expandable is collapsed and the content is not in the DOM.
        /// </summary>
        None,

        /// <summary>
        /// The expandable is collapsed, but the DOM has been rendered and is just not visible.
        /// All contents are clipped.
        /// </summary>
        Collapsed,

        /// <summary>
        /// The expandable is in the period of animation expanding the content.
        /// All contents are clipped.
        /// </summary>
        Expanding,

        /// <summary>
        /// The expandable is completely expanded and shown.
        /// Contents are no longer clipped, allowing forms to be shown outside of bounds.
        /// </summary>
        Expanded,

        /// <summary>
        /// The expandable is in the period of animation collapsing the content.
        /// All contents are clipped.
        /// </summary>
        Collapsing,
    }
}
