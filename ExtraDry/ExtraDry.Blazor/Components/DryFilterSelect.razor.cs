#nullable enable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExtraDry.Blazor;

public partial class DryFilterSelect : ComponentBase {

    [Parameter]
    public PropertyDescription? Property { get; set; }

    private DryExpandable Expandable { get; set; } = null!; // set in page-side partial

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    [Parameter]
    public EventCallback<FilterChangedEventArgs> FilterChanged { get; set; }

    public async Task ToggleForm()
    {
        await Expandable.Toggle();
        StateHasChanged();
    }

    private Stopwatch stopwatch = new Stopwatch();

    public async Task OnFocusOut(FocusEventArgs args)
    {
        stopwatch.Restart();
        shouldCollapse = true;
        // wait and see if we should ignore the out because we're switching focus within control.
        await Task.Delay(1);
        if(shouldCollapse) {
            await EventsAndRefresh();
            shouldCollapse = false;
        }
    }

    private async Task EventsAndRefresh()
    {
        var args = new FilterChangedEventArgs {
            FilterName = Property?.Property?.Name?.ToLowerInvariant() ?? "",
            FilterExpression = FilterString,
        };
        args.FilterValues.AddRange(Selection);
        await FilterChanged.InvokeAsync(args);
        await Expandable.Collapse();
    }

    public void OnChange(ChangeEventArgs args, ValueDescription value)
    {
        var key = value.Key.ToString();
        if(key == null) {
            return;
        }
        if(args?.Value?.Equals(true) ?? false) {
            if(!Selection.Contains(key)) {
                Selection.Add(key);
            }
        }
        else {
            if(Selection.Contains(key)) {
                Selection.Remove(key);
            }
        }
        Console.WriteLine(FilterString);
    }

    public Task OnFocusIn(FocusEventArgs args)
    {
        shouldCollapse = false;
        return Task.CompletedTask;
    }

    private bool shouldCollapse = false;

    public async Task OnKeyDown(KeyboardEventArgs args)
    {
        await EventsAndRefresh();
    }

    private string CssClass => $"filter {Property?.Property?.Name?.ToLowerInvariant()} {Property?.Property?.PropertyType?.ToString()?.ToLowerInvariant()}";

    public List<string> Selection { get; } = new();

    public string FilterString {
        get {
            if(Property == null || !Selection.Any()) {
                return "";
            }
            else {
                return $"{Property?.Property?.Name}:{string.Join('|', Selection)}";
            }
        }
    }

}

public class FilterChangedEventArgs : EventArgs {

    public string FilterName { get; set; } = string.Empty;

    public List<string> FilterValues { get; set; } = new();

    public string FilterExpression { get; set; } = string.Empty;


}
