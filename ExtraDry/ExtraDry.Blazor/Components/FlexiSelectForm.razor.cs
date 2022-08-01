#nullable enable

namespace ExtraDry.Blazor;

public partial class FlexiSelectForm<T> : ComponentBase {

    public FlexiSelectForm()
    {
        Name = $"FlexiSingleSelect{++count}";
    }

    //[Parameter]
    //public EventCallback<SelectMiniDialogChangedEventArgs> FilterChanged { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int ShowFilterThreshold { get; set; } = 10;

    [Parameter]
    public bool ShowSubtitle { get; set; }

    [Parameter]
    public bool ShowImages { get; set; }

    [Parameter]
    public string FilterPlaceholder { get; set; } = "filter";

    [Parameter]
    public string Filter {
        get => filter; 
        set {
            Console.WriteLine($"Filter: {value}");
            filter = value;
            Filters = filter.Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
            foreach(var item in DisplayData) {
                item.FilterClass = ApplyFilter(item.DisplayText);
            }
            StateHasChanged();
        } 
    }
    private string filter = string.Empty;

    [Parameter]
    public IEnumerable<T>? Data { get; set; }

    [Parameter]
    public IEnumerable<T>? Values { get; set; }

    [Parameter]
    public T? SelectedValue { get; set; }

    [Parameter]
    public bool MultiSelect { get; set; }

    public IEnumerable<T> SelectedValues { 
        get {
            foreach(var item in DisplayData) {
                if(item.Selected) {
                    yield return item.Source;
                }
            }
        }
    }

    protected override void OnParametersSet()
    {
        int id = 0;
        if(Data != null && !DisplayData.Any()) {
            foreach(var item in Data) {
                if(item != null) {
                    var displayItem = new DisplayItem(++id, item);
                    displayItem.FilterClass = ApplyFilter(displayItem.DisplayText);
                    DisplayData.Add(displayItem);
                }
            }
        }
    }

    private string[] Filters { get; set; } = Array.Empty<string>();

    private string ApplyFilter(string value)
    {
        if(!Filters.Any()) {
            return "unfiltered";
        }
        else if(Filters.Count() == 1 && Filters.First().Equals("checked", StringComparison.OrdinalIgnoreCase)) {
            var match = DisplayData.Any(e => e.DisplayText == value);
            return match ? "unfiltered" : "filtered";
        }
        else if(Filters.Count() == 1 && Filters.First().Equals("unchecked", StringComparison.OrdinalIgnoreCase)) {
            var match = DisplayData.Any(e => e.ToString() == value);
            return match ? "filtered" : "unfiltered";
        }
        else {
            var match = Filters.Any(e => value.Contains(e, StringComparison.OrdinalIgnoreCase));
            return match ? "unfiltered" : "filtered";
        }
    }

    private void OnChange(ChangeEventArgs args, DisplayItem value)
    {
        if(value == null) {
            return;
        }
        if(args?.Value?.Equals("on") ?? false) {
            // Single select change.
            if(SelectedValue != null) {
                DisplayData.First(e => e.Source?.Equals(SelectedValue) ?? false).Selected = false;
            }
            value.Selected = true;
            SelectedValue = value.Source;
            Console.WriteLine($"Set {value.DisplayText}");
        }
        else if(args?.Value?.Equals(true) ?? false) {
            if(value.Selected == false) {
                value.Selected = true;
                Console.WriteLine($"Added {value.DisplayText}");
            }
        }
        else {
            if(value.Selected == true) {
                value.Selected = false;
                Console.WriteLine($"Removed {value.DisplayText}");
            }
        }
    }

    private TriCheckState TriCheckValue {
        get {
            if(!DisplayData.Any(e => e.Selected)) {
                return TriCheckState.Unchecked;
            }
            else if(DisplayData.All(e => e.Selected)) {
                return TriCheckState.Checked;
            }
            else {
                return TriCheckState.Indeterminate;
            }
        }
    }

    private void SelectAllChange(ChangeEventArgs args)
    {
        if(args.Value is TriCheckState tri) {
            if(tri == TriCheckState.Checked) {
                CheckAll(true);
            }
            else if(tri == TriCheckState.Unchecked) {
                CheckAll(false);
            }
        }
    }

    public void ClearAll(MouseEventArgs _)
    {
        CheckAll(false);
    }

    private void CheckAll(bool value)
    {
        foreach(var item in DisplayData) {
            item.Selected = value;
        }
        StateHasChanged();
    }

    private void ClearFilter(MouseEventArgs _)
    {
        Filter = "";
        StateHasChanged();
    }

    private List<DisplayItem> DisplayData { get; set; } = new();

    private string Name { get; set; }

    private static int count = 0;

    private class DisplayItem
    {
        public DisplayItem(int id, T item)
        {
            Id = $"item{id}";
            DisplayText = item?.ToString() ?? "unnamed";
            Source = item;
            if(item is IFlexiSelectItem flexiValue) {
                CssClass = flexiValue.CssClass;
                DisplayText = flexiValue.Title;
                Subtitle = flexiValue.Subtitle;
                Thumbnail = flexiValue.Thumbnail;
            }
        }

        public string Id { get; set; }

        public string CssClass { get; set; } = string.Empty;

        public string DisplayText { get; set; }

        public string? Subtitle { get; set; } = string.Empty;

        public string? Thumbnail { get; set; } = string.Empty;
        
        public string FilterClass { get; set; } = "unfiltered";

        public T Source { get; }

        public bool Selected { get; set; }

        public string SelectClass => Selected ? "selected" : "";

    }

}
