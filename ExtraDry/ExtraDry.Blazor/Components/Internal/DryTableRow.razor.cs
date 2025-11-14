using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor.Components.Internal;

[SuppressMessage("Usage", "DRY1500:Extra DRY Blazor components should have an interface.", Justification = "Internal component not for general use.")]
public partial class DryTableRow<T> : ComponentBase, IDisposable
{
    /// <summary>
    /// Required parameter which is the view model description passed from the DryTable.
    /// </summary>
    [Parameter, EditorRequired]
    public DecoratorInfo Decorator { get; set; } = null!; // Only used in DryTable

    /// <summary>
    /// Required parameter which is the selection set for all items, passed from the DryTable.
    /// </summary>
    [Parameter, EditorRequired]
    public SelectionSet Selection { get; set; } = null!; // Only used in DryTable

    /// <summary>
    /// Required parameter which is the current item, passed from the DryTable.
    /// </summary>
    [Parameter, EditorRequired]
    public ListItemInfo<T> Item { get; set; } = null!; // Only used in DryTable

    [Parameter]
    public string GroupColumn { get; set; } = null!; // Only used in DryTable

    /// <summary>
    /// Indicates if command buttons should be shown for this row.  Commands must be contextual
    /// commands to appear here.  That is, they take a single item as a parameter.
    /// </summary>
    [Parameter]
    public bool ShowCommands { get; set; } = true;

    [Parameter]
    public int Height { get; set; } = 40;

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private string LevelCss => Item.Item is IHierarchyEntity ? $"level-{Item.GroupDepth}" : "";

    private bool initialized;

    protected override void OnParametersSet()
    {
        if(Decorator == null) {
            throw new InvalidOperationException("The parameter `Decorator` is required in DryTableRow.");
        }
        if(Selection == null) {
            throw new InvalidOperationException("The parameter `Selection` is required in DryTableRow.");
        }
        if(Item == null) {
            throw new InvalidOperationException("The parameter `Item` is required in DryTableRow.");
        }
        if(!initialized) {
            initialized = true;
            Selection.Changed += Selection_Changed;
        }
    }

    private string ClickableClass => Decorator.ListSelectMode == ListSelectMode.Action ? "clickable" : "";

    private string SelectedClass => IsSelected ? "selected" : "";

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, ClickableClass, SelectedClass, LevelCss);

    private string RadioButtonScope => $"{Decorator.GetHashCode()}";

    private bool IsSelected => Item.Item != null && Selection.Contains(Item.Item);

    private string UuidValue => Decorator.UuidProperty?.GetValue(Item.Item)?.ToString() ?? string.Empty;

    private async Task RowClick(MouseEventArgs args)
    {
        if(Decorator.ListSelectMode == ListSelectMode.Action) {
            if(Decorator.SelectCommand != null && Item.Item != null) {
                await Decorator.SelectCommand.ExecuteAsync(Item.Item);
            }
        }
        if(args.CtrlKey) {
            // same as clicking the checkbox
            CheckChanged(new());
        }
        else if(args.ShiftKey) {

        }
        else {
            Selection.Clear();
            Select();
        }
    }

    private async Task RowDoubleClick(MouseEventArgs _)
    {
        if(Decorator.DefaultCommand != null && Item.Item != null) {
            await Decorator.DefaultCommand.ExecuteAsync(Item.Item);
        }
        StateHasChanged();
    }

    private void CheckboxClick(MouseEventArgs _)
    {
        CheckChanged(new());
    }

    private void CheckChanged(ChangeEventArgs _)
    {
        if(IsSelected) {
            Deselect();
        }
        else {
            Select();
        }
        StateHasChanged();
    }

    private void Select()
    {
        if(Item.Item != null) {
            Selection.Add(Item.Item);
        }
    }

    private void Deselect()
    {
        if(Item.Item != null) {
            Selection.Remove(Item.Item);
        }
    }

    private void Selection_Changed(object? sender, SelectionSetChangedEventArgs args)
    {
        StateHasChanged(); // external event, need to signal to update UI.
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Selection.Changed -= Selection_Changed;
    }
}
