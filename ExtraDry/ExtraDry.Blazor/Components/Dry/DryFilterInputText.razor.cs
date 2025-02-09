namespace ExtraDry.Blazor;

/// <summary>
/// A filter component used by the DryFilter to display text input that is passed directly to the
/// PageQuery in the API, used for keyword selections.
/// </summary>
public partial class DryFilterInputText : ComponentBase, IExtraDryComponent, IDisposable
{
    /// <inheritdoc cref="IExtraDryComponent.CssClass " />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IComments.Placeholder" />
    [Parameter]
    public string Placeholder { get; set; } = "filter by keyword...";

    [Parameter]
    public string Icon { get; set; } = "";

    [Parameter]
    public bool ShowIcon { get; set; } = true;

    [Parameter]
    public string ResetIcon { get; set; } = "times";

    [Parameter]
    public bool ShowReset { get; set; } = false;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <inheritdoc cref="DryPageQueryView.PageQueryBuilder" />
    [CascadingParameter]
    internal QueryBuilder? PageQueryBuilder { get; set; }

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
        GC.SuppressFinalize(this);
        if(PageQueryBuilder != null) {
            PageQueryBuilder.OnChanged -= PageQueryBuilder_OnChanged;
        }
    }

    private bool DisplayIcon => ShowIcon && !string.IsNullOrWhiteSpace(Icon);

    private bool DisplayReset => ShowReset && !string.IsNullOrWhiteSpace(ResetIcon);

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

    private async Task OnReset()
    {
        if(!string.IsNullOrWhiteSpace(FreeTextFilter)) {
            FreeTextFilter = "";
            StateHasChanged();
            await Task.Delay(1); // let the input update
            if(PageQueryBuilder != null) {
                PageQueryBuilder.TextFilter.Keywords = FreeTextFilter;
                filterInSync = false;
                if(string.IsNullOrWhiteSpace(FreeTextFilter)) {
                    SyncPageQueryBuilder();
                }
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
