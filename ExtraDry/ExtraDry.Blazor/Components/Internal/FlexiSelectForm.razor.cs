namespace ExtraDry.Blazor.Internal;

/// <summary>
/// The internal form for the Flexi-Select component. 
/// Do not use directly.
/// </summary>
public partial class FlexiSelectForm<TItem> : ComponentBase, IExtraDryComponent where TItem : notnull {

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The number of items in `Data` that will trigger the filter to show.
    /// Use 0 to always show and Int.MaxValue to never show.
    /// Default is to show after 10 items are in the form.
    /// </summary>
    [Parameter]
    public int ShowFilterThreshold { get; set; } = 10;

    /// <summary>
    /// Sets the `ShowSubtitle` property on all child `MiniCard`s.
    /// </summary>
    [Parameter]
    public bool? ShowSubtitle { get; set; } = null;

    /// <summary>
    /// Sets the `ShowImages` property on all child `MiniCard`s.
    /// </summary>
    [Parameter]
    public bool? ShowThumbnail { get; set; }

    /// <summary>
    /// The text to use as a placeholder for filters when the filter field is shown.
    /// </summary>
    [Parameter]
    public string FilterPlaceholder { get; set; } = "filter";

    /// <summary>
    /// The list of all possibly source value to display in the component.
    /// </summary>
    [Parameter]
    public IEnumerable<TItem>? Data { get; set; }

    /// <summary>
    /// Indicates if the select control should be single- or multi- select.
    /// If single, bind to `Value`, if multi, bind to `Values`.
    /// </summary>
    [Parameter]
    public bool MultiSelect { get; set; }

    /// <inheritdoc cref="IComments{TItem}.Value" />
    [Parameter]
    public TItem? Value { get; set; }

    /// <inheritdoc cref="IComments{TItem}.ValueChanged" />
    [Parameter]
    public EventCallback<TItem> ValueChanged { get; set; }

    /// <summary>
    /// The selected values when the component is set to multi-select mode.
    /// Use with two-way data binding.
    /// </summary>
    [Parameter]
    public List<TItem>? Values { get; set; }

    /// <summary>
    /// The changed event for `Values` for use with two-way data binding.
    /// </summary>
    [Parameter]
    public EventCallback<List<TItem>?> ValuesChanged { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    protected override async void OnParametersSet()
    {
        int id = 0;
        if(Data != null && DisplayData.Count == 0) {
            foreach(var item in Data) {
                if(item != null) {
                    var displayItem = new DisplayItemViewModel(++id, item);
                    ApplyFilter(displayItem);
                    DisplayData.Add(displayItem);
                    if(MultiSelect && (Values?.Contains(item) ?? false)) {
                        displayItem.Selected = true;
                    }
                    if(!MultiSelect && item.Equals(Value)) {
                        displayItem.Selected = true;
                    }
                }
            }
        }
        if(MultiSelect) {
            if(Value != null) {
                Value = default;
                await ValueChanged.InvokeAsync(Value);
            }
            if(Values == null) {
                Values = [];
                foreach(var item in DisplayData) {
                    item.Selected = false;
                }
                await ValuesChanged.InvokeAsync(Values);
            }
        }
        else {
            if(Values != null) {
                Values = null;
                foreach(var item in DisplayData) {
                    item.Selected = false;
                }
                await ValuesChanged.InvokeAsync(Values);
            }
        }
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "flexi-select-form", CssClass);

    private bool ShowFilter => ShowFilterThreshold < Data?.Count();

    /// <summary>
    /// The filter string for binding to the input filter.
    /// </summary>
    private string Filter {
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

    private string[] Filters { get; set; } = [];

    private void ApplyFilter(DisplayItemViewModel value)
    {
        bool match = true;
        if(Filters.Length == 0) {
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

    private async void OnChange(ChangeEventArgs args, DisplayItemViewModel value)
    {
        if(value == null) {
            return;
        }
        if(args?.Value?.Equals("on") ?? false) {
            // Single select change.
            foreach(var item in DisplayData) {
                // Probably more efficent to set all to not selected than to do half as many complex comparisons.
                item.Selected = false;
            }
            value.Selected = true;
            Value = value.Source;
            await ValueChanged.InvokeAsync(Value);
        }
        else if(args?.Value?.Equals(true) ?? false) {
            // Multi-select add.
            if(value.Selected == false) {
                value.Selected = true;
                Values?.Add(value.Source);
                await ValuesChanged.InvokeAsync(Values);
            }
        }
        else {
            // Multi-select remove.
            if(value.Selected == true) {
                value.Selected = false;
                Values?.Remove(value.Source);
                await ValuesChanged.InvokeAsync(Values);
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

    private async void SelectAllChange(ChangeEventArgs args)
    {
        if(args.Value is TriCheckState tri) {
            if(tri == TriCheckState.Checked) {
                await CheckAll(true);
            }
            else if(tri == TriCheckState.Unchecked) {
                await CheckAll(false);
            }
        }
    }

    private async void ClearAll(MouseEventArgs _)
    {
        await CheckAll(false);
    }

    private async Task CheckAll(bool value)
    {
        if(Values == null) {
            // Single select mode, shouldn't happen.
            return;
        }
        foreach(var item in DisplayData) {
            item.Selected = value;
        }
        Values.Clear();
        if(value == true && Data != null) {
            Values.AddRange(Data);
        }
        StateHasChanged();
        await ValuesChanged.InvokeAsync(Values);
    }

    private void ClearFilter(MouseEventArgs _)
    {
        Filter = "";
        StateHasChanged();
    }

    private List<DisplayItemViewModel> DisplayData { get; set; } = [];

    /// <summary>
    /// Use as name on radio buttons to get HTML to enforce mutually exclusive selections.
    /// </summary>
    private string Name { get; set; } = $"FlexiSingleSelect{++count}";

    private static int count;

    private class DisplayItemViewModel
    {
        public DisplayItemViewModel(int id, TItem item)
        {
            Id = $"item{id}";
            Title = item?.ToString() ?? "unnamed";
            Source = item!; // TItem is notnull, not sure why had to override warning here.
            if(item is ISubjectViewModel subject) {
                Title = subject.Title;
                Subtitle = subject.Subtitle;
            }
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; } = string.Empty;

        public string FilterClass { get; set; } = "unfiltered";

        public TItem Source { get; }

        public bool Selected { get; set; }

        public string SelectClass => Selected ? "selected" : "";

    }

}
