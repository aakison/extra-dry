namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A representation of a fieldset in a form, typically rendered automatically by an enclosing <see
/// cref="DryForm{T}" />. This component may be used to group a set of properties together on a
/// form with a legend. It may also be used to group a set of child items in a list, where each has
/// their own small form or rendering.
/// </summary>
public partial class DryFieldset : ComponentBase, IExtraDryComponent
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <summary>
    /// Cascading parameter so the model can be internal and not exposed to users.
    /// </summary>
    [CascadingParameter]
    internal IDryForm Form { get; set; } = null!;

    /// <summary>
    /// Cascading parameter so the model can be internal and not exposed to users.
    /// </summary>
    //[CascadingParameter]
    private FormFieldset? FormFieldset { get; set; }

    [Parameter]
    public string GroupName { get; set; } = "";

    protected override void OnParametersSet()
    {
        FormFieldset = Form.FormDescription?.Fieldsets?.FirstOrDefault(e => string.Equals(e.Name, GroupName, StringComparison.OrdinalIgnoreCase));
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-fieldset", FormFieldset?.Name, Form.ModelNameSlug, FormFieldset?.CssClass, CssClass);

    private CommandInfo AddNewCommand =>
        new(this, MethodInfoHelper.GetMethodInfo<DryFieldset>(e => e.AddDefaultElementToList(Array.Empty<int>()))) {
            Arguments = CommandArguments.Single,
            Context = CommandContext.Regular
        };

    [Command(Name = "Add New", Icon = "plus")]
    private void AddDefaultElementToList(IList items)
    {
        var type = items.GetType().SingleGenericType();
        var instance = type.CreateDefaultInstance();
        items.Add(instance);
        if(Form.Description != null && Form.UntypedModel != null) {
            Form.FormDescription = new FormDescription(Form.Description, Form.UntypedModel); // re-build description to add/remove UI elements.
        }
        StateHasChanged();
    }

    private void DeleteItem(object? items, object item)
    {
        Console.WriteLine("DeleteItem");
        ArgumentNullException.ThrowIfNull(items);
        if(items is IList list) {
            Console.WriteLine("  A list");
            list.Remove(item);
            if(Form.Description != null && Form.UntypedModel != null) {
                Form.FormDescription = new FormDescription(Form.Description, Form.UntypedModel); // re-build description to add/remove UI elements.
            }
            StateHasChanged();
        }
        else {
            Console.WriteLine("  Not a list: " + items.GetType().ToString());
        }
    }
}
