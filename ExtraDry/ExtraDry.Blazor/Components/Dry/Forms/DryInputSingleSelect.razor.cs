using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a single select dropdown list. Prefer the use of <see cref="DryInput{T}"
/// /> instead of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputSingleSelect<T>
    : DryInputBase<T>, IDryInput<T>, IExtraDryComponent
    where T : class
{
    /// <summary>
    /// Set of values to select from, any object can be used and the display text is either
    /// IResourceIdentifiers.Title or object.ToString() value.
    /// </summary>
    [Parameter, EditorRequired]
    public List<object> Values { get; set; } = null!;

    protected override void OnParametersSet()
    {
        SelectedValue = Property.GetValue(Model);
        if(SelectedValue is Guid uuid && Property.InputType.IsClass) {
            SelectedValue = Values.SingleOrDefault(e => (e as IResourceIdentifiers)?.Uuid == uuid);
        }
        Options = Values.Select((e, i) => new DryInputOption(e, i)).ToList();
        if(SelectedValue is IResourceIdentifiers resource) {
            SelectedOption = Options.FirstOrDefault(e => e.Key == resource.Uuid.ToString());
        }
        else if(SelectedValue != null) {
            SelectedOption = Options.FirstOrDefault(e => e.Value.Equals(SelectedValue));
        }
    }

    private async Task SelectOption(ChangeEventArgs args)
    {
        if(Values == null || string.IsNullOrEmpty(args.Value?.ToString())) {
            return; // Selected blank line or invalid value
        }
        SelectedOption = Options.FirstOrDefault(e => e.Key == args.Value.ToString());
        SelectedValue = SelectedOption.Value;
        if(Model != null) {
            Property?.SetValue(Model, SelectedValue);
            await OnChange.InvokeAsync(args);
        }
    }

    private bool ReadOnly => EditMode == EditMode.ReadOnly;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "select", ReadOnlyCss, CssClass);

    private object? SelectedValue { get; set; }

    private List<DryInputOption> Options { get; set; } = [];

    private DryInputOption? SelectedOption { get; set; }

    public class DryInputOption { 
    
        public DryInputOption(object source, int index)
        {
            if(source is IResourceIdentifiers resource) {
                Key = resource.Uuid.ToString();
                DisplayText = resource.Title ?? source.ToString() ?? "--empty--";
            } else {
                Key = Guid.NewGuid().ToString();
                DisplayText = source.ToString() ?? "--empty--";
            }
            Value = source;
            Index = index;
        }

        public string Key { get; init; }

        public string DisplayText { get; init; }

        public int Index { get; init; }

        public object Value { get; init; }
    }

}
