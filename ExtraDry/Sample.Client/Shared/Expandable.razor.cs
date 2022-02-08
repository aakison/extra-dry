#nullable enable

using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Sample.Client.Shared;

public partial class Expandable {
    
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
        //if(State == ExpandedState.None) {
        //    State = ExpandedState.Collapsed;
        //    StateHasChanged();
        //    await Task.Delay(minimumDuration);
        //}
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

    public ExpandedState State { get; set; }

    private const int minimumDuration = 15; // One frame to allow refresh to happen.

    private int AdjustedDuration => Math.Clamp(Duration, minimumDuration, 3000);

    private string InlineStyle => $"transition: height {AdjustedDuration / 1000.0}s; height: {HeightString}";

    private string HeightString => State switch {
        ExpandedState.Collapsed => "0",
        ExpandedState.Collapsing => "0",
        _ => $"{Height}px",
    };

    private string CssClassState => State.ToString().ToLowerInvariant();

    public enum ExpandedState {
        Collapsed,
        Expanding,
        Expanded,
        Collapsing,
    }
}
