namespace ExtraDry.Blazor;

public partial class DryFilter<TItem> : ComponentBase {

    public DryFilter()
    {
        ViewModelDescription = new ViewModelDescription(typeof(TItem), this);
    }

    /// <inheritdoc cref="IComments.Placeholder"/>
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
        //Console.WriteLine($"OnKeyPress {args.Key}: {FreeTextFilter} on {Query}");
        if(args.Key == "Enter") {
            SyncQuery();
        }
    }

    private void OnFocusOut(FocusEventArgs args)
    {
        //Console.WriteLine($"OnFocusOut: {args.Type}: {FreeTextFilter} on {Query}");
        SyncQuery();
    }

    private void SyncQuery()
    {
        if(Query != null) {
            //Console.WriteLine("Setting query");
            Query.Filter = FilterExpression;
            StateHasChanged();
            if(Query is NotifyPageQuery notify) {
                notify.NotifyChanged();
            }
        }
    }

    public void FilterChanged(FilterChangedEventArgs args)
    {
        Console.WriteLine("FilterChanged");
        if(args.FilterValues.Any()) {
            if(!SelectFilters.ContainsKey(args.FilterName)) {
                SelectFilters.Add(args.FilterName, args.FilterExpression);
            }
            else {
                SelectFilters[args.FilterName] = args.FilterExpression;
            }
            SyncQuery();
        }
        else {
            if(SelectFilters.ContainsKey(args.FilterName)) {
                SelectFilters.Remove(args.FilterName);
            }
            SyncQuery();
        }
    }

    private string FreeTextFilter { get; set; } = string.Empty;

    private Dictionary<string, string> SelectFilters { get; } = new();

    private string FilterExpression => $"{FreeTextFilter} {string.Join(' ', SelectFilters.Values)}".Trim();

    private ViewModelDescription ViewModelDescription { get; set; }

    private static string TypeSlug => typeof(TItem).Name.ToLowerInvariant();

}
