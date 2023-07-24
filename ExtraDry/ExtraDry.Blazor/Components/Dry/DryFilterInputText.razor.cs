namespace ExtraDry.Blazor;

/// <summary>
/// A filter component used by the DryFilter to display text input that is passed directly to the
/// PageQuery in the API, used for keyword selections.
/// </summary>
public partial class DryFilterInputText : ComponentBase, IExtraDryComponent, IDisposable {

    /// <inheritdoc cref="IExtraDryComponent.CssClass "/>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IComments.Placeholder"/>
    [Parameter]
    public string Placeholder { get; set; } = "filter by keyword...";

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <inheritdoc cref="DryPageQueryView.PageQueryBuilder" />
    [CascadingParameter]
    internal PageQueryBuilder? PageQueryBuilder { get; set; }

    protected override void OnParametersSet()
    {
        if(PageQueryBuilder != null) {
            PageQueryBuilder.OnChanged += PageQueryBuilder_OnChanged;
        }
    }

    private void PageQueryBuilder_OnChanged(object? sender, EventArgs e)
    {
        if(filterInSync && FreeTextFilter != PageQueryBuilder!.TextFilter.Keywords) {
            // component thinks everything sync'd up, but changes made, must be by someone else...
            FreeTextFilter = PageQueryBuilder!.TextFilter.Keywords;
            StateHasChanged();
        }
    }

    public void Dispose()
    {
        if(PageQueryBuilder != null) {
            PageQueryBuilder.OnChanged -= PageQueryBuilder_OnChanged;
        }
    }

    /// <summary>
    /// When the keyword input changes, update the PageQueryBuilder with the new keyword values.
    /// Normally a single keystroke won't trigger a change notification, but if the keywords are
    /// erased completely, trigger the notification on any input instead of just on 'enter'.
    /// </summary>
    private void OnInput(ChangeEventArgs args)
    {
        if(PageQueryBuilder != null) {
            FreeTextFilter = $"{args.Value}";
            PageQueryBuilder.TextFilter.Keywords = FreeTextFilter;
            filterInSync = false;
            if(string.IsNullOrWhiteSpace(FreeTextFilter)) {
                SyncPageQueryBuilder();
            }
        }
    }

    /// <summary>
    /// On an explicit 'enter', refresh the associated page query builder if necessary.
    /// </summary>
    private void OnKeyPress(KeyboardEventArgs args)
    {
        if(args.Key == "Enter") {
            SyncPageQueryBuilder();
        }
    }

    /// <summary>
    /// When focus is lost update the PageQuery.
    /// </summary>
    private void OnFocusOut(FocusEventArgs args)
    {
        SyncPageQueryBuilder();
    }

    private void SyncPageQueryBuilder()
    {
        if(!filterInSync) {
            PageQueryBuilder?.NotifyChanged();
            filterInSync = true;
        }
    }

    private string FreeTextFilter { get; set; } = string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-field-filter", "filter-text", "keywords", CssClass);

    private bool filterInSync = true;

}
