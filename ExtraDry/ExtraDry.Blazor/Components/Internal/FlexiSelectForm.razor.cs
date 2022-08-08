#nullable enable

namespace ExtraDry.Blazor.Internal;

public partial class FlexiSelectForm<TItem> : ComponentBase {

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
            filter = value;
            Filters = filter.Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
            foreach(var item in DisplayData) {
                ApplyFilter(item);
            }
            StateHasChanged();
        } 
    }
    private string filter = string.Empty;

    [Parameter]
    public IEnumerable<TItem>? Data { get; set; }

    [Parameter]
    public IEnumerable<TItem>? Values { get; set; }

    [Parameter] 
    public TItem? Value { get; set; }

    [Parameter]
    public bool MultiSelect { get; set; }

    [Parameter]
    public EventCallback<TItem> ValueChanged { get; set; }

    public IEnumerable<TItem> SelectedValues { 
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
                    ApplyFilter(displayItem);
                    DisplayData.Add(displayItem);
                }
            }
        }
        if(Value != null) {
            var selected = DisplayData.FirstOrDefault(e => Value.Equals(e.Source));
            if(selected != null) {
                selected.Selected = true;
            }
        }
    }

    private string[] Filters { get; set; } = Array.Empty<string>();

    private void ApplyFilter(DisplayItem value)
    {
        bool match = true;
        if(!Filters.Any()) {
            match = true;
        }
        else if(Filters.Length == 1 && Filters.First().Equals("checked", StringComparison.OrdinalIgnoreCase)) {
            match = value.Selected;
        }
        else if(Filters.Length == 1 && Filters.First().Equals("unchecked", StringComparison.OrdinalIgnoreCase)) {
            match = !value.Selected;
        }
        else {
            match = Filters.Any(e => value.Title.Contains(e, StringComparison.OrdinalIgnoreCase) 
                || value.Subtitle.Contains(e, StringComparison.OrdinalIgnoreCase));
        }
        value.FilterClass = match ? "unfiltered" : "filtered";
    }

    private async void OnChange(ChangeEventArgs args, DisplayItem value)
    {
        if(value == null) {
            return;
        }
        if(args?.Value?.Equals("on") ?? false) {
            // Single select change.
            if(Value != null) {
                DisplayData.First(e => e.Source?.Equals(Value) ?? false).Selected = false;
            }
            value.Selected = true;
            Value = value.Source;
            await ValueChanged.InvokeAsync(Value);
            Console.WriteLine($"Set {value.Title}");
        }
        else if(args?.Value?.Equals(true) ?? false) {
            if(value.Selected == false) {
                value.Selected = true;
                Console.WriteLine($"Added {value.Title}");
            }
        }
        else {
            if(value.Selected == true) {
                value.Selected = false;
                Console.WriteLine($"Removed {value.Title}");
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
        public DisplayItem(int id, TItem item)
        {
            Id = $"item{id}";
            Title = item?.ToString() ?? "unnamed";
            Source = item;
            if(item is ISubjectViewModel subject) {
                Thumbnail = subject.Thumbnail;
                Title = subject.Title;
                Subtitle = subject.Subtitle;
            }
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; } = string.Empty;

        public string Thumbnail { get; set; } = string.Empty;
        
        public string FilterClass { get; set; } = "unfiltered";

        public TItem Source { get; }

        public bool Selected { get; set; }

        public string SelectClass => Selected ? "selected" : "";

    }

}
