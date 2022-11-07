#nullable disable

namespace ExtraDry.Blazor.Components.Internal;

public partial class ViewModelTableRow<T> : ComponentBase, IDisposable {

    /// <summary>
    /// Required parameter which is the view model description passed from the DryTable.
    /// </summary>
    [Parameter]
    public ViewModelDescription Description { get; set; }

    /// <summary>
    /// Required parameter which is the selection set for all items, passed from the DryTable.
    /// </summary>
    [Parameter]
    public SelectionSet Selection { get; set; }

    /// <summary>
    /// Required parameter which is the current item, passed from the DryTable.
    /// </summary>
    [Parameter]
    public ListItemInfo<T> Item { get; set; }

    [Parameter]
    public Grouping Grouping { get; set; }

    [Parameter]
    public int Height { get; set; } = 40;

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Inject]
    private ILogger<ViewModelTableRow<T>> Logger { get; set; }

    protected override void OnParametersSet()
    {
        if(Description == null) {
            throw new InvalidOperationException("The parameter `Description` is required in ViewModelTableRow.");
        }
        if(Selection == null) {
            throw new InvalidOperationException("The parameter `Selection` is required in ViewModelTableRow.");
        }
        if(Item == null) {
            throw new InvalidOperationException("The parameter `Item` is required in ViewModelTableRow.");
        }
    }

    private string ClickableClass => Description.ListSelectMode == ListSelectMode.Action ? "clickable" : "";

    private string SelectedClass => IsSelected ? "selected" : "";

    private string CssClasses => DataConverter.JoinNonEmpty(CssClass, ClickableClass, SelectedClass);

    private string RadioButtonScope => Description.GetHashCode().ToString();

    private bool IsSelected => Selection.Contains(Item.Item);

    private void RowClick(MouseEventArgs _)
    {
        Logger.LogInformation("Select Row with Row Click");
        if(Description.ListSelectMode == ListSelectMode.Action) {
            Description.SelectCommand?.ExecuteAsync(Item);
        }
        else if(IsSelected) {
            Deselect();
        }
        else {
            Select();
        }
        StateHasChanged();
    }

    private void CheckChanged(ChangeEventArgs args)
    {
        Logger.LogInformation("Checked checkbox/radio with new value '{arg}'", args?.Value);
        //if(IsSelected) {
        //    Select();
        //}
        //else {
        //    Deselect();
        //}
        //StateHasChanged();
    }

    private void Select()
    {
        if(!IsSelected) {
            Selection.Add(Item.Item);
            if(!Selection.MultipleSelect) {
                Selection.Changed += OnExclusivity;
            }
        }
    }

    private void Deselect()
    {
        if(IsSelected) {
            Selection.Remove(Item.Item);
            if(!Selection.MultipleSelect) {
                Selection.Changed -= OnExclusivity;
            }
        }
    }

    private void OnExclusivity(object sender, EventArgs args)
    {
        Deselect();
        StateHasChanged(); // external event, need to signal to update UI.
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Selection.Changed -= OnExclusivity;
    }

}
