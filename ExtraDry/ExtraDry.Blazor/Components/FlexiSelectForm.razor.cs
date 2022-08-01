#nullable enable

using System.Collections.ObjectModel;

namespace ExtraDry.Blazor;

public partial class FlexiSelectForm<T> : ComponentBase {

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
            Console.WriteLine($"Value: {Filter}");
            Filters = filter.Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
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

    public ObservableCollection<T> SelectedValues { get; private set; } = new();

    private string[] Filters { get; set; } = Array.Empty<string>();

    private string ApplyFilter(string value)
    {
        if(!Filters.Any()) {
            return "unfiltered";
        }
        else if(Filters.Count() == 1 && Filters.First().Equals("checked", StringComparison.OrdinalIgnoreCase)) {
            var match = SelectedValues.Any(e => e.ToString() == value);
            return match ? "unfiltered" : "filtered";
        }
        else if(Filters.Count() == 1 && Filters.First().Equals("unchecked", StringComparison.OrdinalIgnoreCase)) {
            var match = SelectedValues.Any(e => e.ToString() == value);
            return match ? "filtered" : "unfiltered";
        }
        else {
            var match = Filters.Any(e => value.Contains(e, StringComparison.OrdinalIgnoreCase));
            return match ? "unfiltered" : "filtered";
        }
    }

    private void OnChange(ChangeEventArgs args, T value)
    {
        if(value == null) {
            return;
        }
        if(args?.Value?.Equals(true) ?? false) {
            if(!SelectedValues.Contains(value)) {
                SelectedValues.Add(value);
                Console.WriteLine($"Added {value}");
            }
        }
        else {
            if(SelectedValues.Contains(value)) {
                SelectedValues.Remove(value);
                Console.WriteLine($"Removed {value}");
            }
        }
    }

    private TriCheckState TriCheckValue {
        get {
            if(!SelectedValues.Any()) {
                return TriCheckState.Unchecked;
            }
            else if(SelectedValues.Count == Data?.Count()) {
                return TriCheckState.Checked;
            }
            else {
                return TriCheckState.Indeterminate;
            }
        }
    }

    public void ClearAll(MouseEventArgs _)
    {
        Console.WriteLine("asdf");
        SelectedValues.Clear();
        StateHasChanged();
    }

    private bool IsSelected(DisplayItem item)
    {
        Console.WriteLine($"Checking {item.DisplayText}");
        return SelectedValues.Contains(item.Source);
    }

    private void ClearFilter(MouseEventArgs _)
    {
        Filter = "";
        StateHasChanged();
    }

    private IEnumerable<DisplayItem> DisplayData()
    {
        int id = 0;
        if(Data != null) {
            foreach(var item in Data) {
                if(item != null) {
                    var displayItem = new DisplayItem(++id, item);
                    displayItem.FilterClass = ApplyFilter(displayItem.DisplayText);
                    yield return displayItem;
                }
            }
        }
    }

    private string PopulatedClass => SelectedValues.Any() ? "active" : "inactive";

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
    }

}
