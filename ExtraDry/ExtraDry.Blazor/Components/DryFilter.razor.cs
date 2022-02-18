#nullable enable

using ExtraDry.Blazor.Internal;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace ExtraDry.Blazor;

public partial class DryFilter<TItem> : ComponentBase {

    public DryFilter()
    {
        ViewModelDescription = new ViewModelDescription(typeof(TItem), this);
    }

    [Parameter]
    public string Placeholder { get; set; } = "filter by keyword...";

    [CascadingParameter]
    public PageQuery? Query { get; set; }

    private void OnInput(ChangeEventArgs args)
    {
        FreeTextFilter = $"{args.Value}";
        if(string.IsNullOrWhiteSpace(FreeTextFilter)) {
            SyncQuery();
        }
    }

    private void OnKeyPress(KeyboardEventArgs args)
    {
        Console.WriteLine($"OnKeyPress {args.Key}: {FreeTextFilter} on {Query}");
        if(args.Key == "Enter") {
            SyncQuery();
        }
    }

    private void OnFocusOut(FocusEventArgs args)
    {
        Console.WriteLine($"OnFocusOut: {args.Type}: {FreeTextFilter} on {Query}");
        SyncQuery();
    }

    private void SyncQuery()
    {
        if(Query != null) {
            Console.WriteLine("Setting query");
            Query.Filter = FreeTextFilter;
            StateHasChanged();
            if(Query is NotifyPageQuery notify) {
                notify.NotifyChanged();
            }
        }
    }

    private string FreeTextFilter { get; set; } = string.Empty;

    private ViewModelDescription ViewModelDescription { get; set; }

    private static string TypeSlug => typeof(TItem).Name.ToLowerInvariant();

}
