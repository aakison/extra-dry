#nullable enable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ExtraDry.Blazor;

public partial class DryFilterSelect : ComponentBase {

    [Parameter]
    public PropertyDescription? Property { get; set; }

    private DryExpandable Expandable { get; set; } = null!; // set in page-side partial

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    public async Task ToggleForm()
    {
        await Expandable.Toggle();
        StateHasChanged();
    }

    private Stopwatch stopwatch = new Stopwatch();

    public async Task OnFocusOut(FocusEventArgs args)
    {
        stopwatch.Restart();
        Console.WriteLine("OnFocusOut");
        shouldCollapse = true;
        // wait and see if we should ignore the out because we're switching focus within control.
        await Task.Delay(1);
        Console.WriteLine($"Should Collapse? {shouldCollapse}");
        if(shouldCollapse) {
            Console.WriteLine("Should Collapse!");
            await Expandable.Collapse();
            shouldCollapse = false;
        }
    }

    public Task OnFocusIn(FocusEventArgs args)
    {
        Console.WriteLine($"OnFocusIn {stopwatch.ElapsedMilliseconds}");
        shouldCollapse = false;
        return Task.CompletedTask;
    }

    private bool shouldCollapse = false;

    public async Task OnKeyDown(KeyboardEventArgs args)
    {
        Console.WriteLine($"OnKeyDown");
        await Expandable.Collapse();
    }

    private string CssClass => $"filter {Property?.Property?.Name?.ToLowerInvariant()} {Property?.Property?.PropertyType?.ToString()?.ToLowerInvariant()}";

}
