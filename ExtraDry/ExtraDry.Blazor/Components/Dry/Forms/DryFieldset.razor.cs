namespace ExtraDry.Blazor.Forms;

public partial class DryFieldset<T> : ComponentBase {

    [CascadingParameter]
    internal DryForm<T> Form { get; set; } = null!;

    [CascadingParameter]
    internal FormFieldset Fieldset { get; set; } = null!;

    private CommandInfo AddNewCommand =>
        new(this, MethodInfoHelper.GetMethodInfo<DryFieldset<T>>(e => e.AddDefaultElementToList(Array.Empty<int>()))) {
            Arguments = CommandArguments.Single,
            Context = CommandContext.Alternate
        };

    [Command(Name = "Add New", Icon = "plus")]
    private void AddDefaultElementToList(IList items)
    {
        var type = items.GetType().SingleGenericType();
        var instance = type.CreateDefaultInstance();
        items.Add(instance);
        if(Form.Description != null && Form.Model != null) {
            Form.FormDescription = new FormDescription(Form.Description, Form.Model); // re-build description to add/remove UI elements.
        }
        StateHasChanged();
    }

    private void DeleteItem(object? items, object item)
    {
        Console.WriteLine("DeleteItem");
        if(items == null) {
            throw new ArgumentNullException(nameof(items));
        }
        if(items is IList list) {
            Console.WriteLine("  A list");
            list.Remove(item);
            if(Form.Description != null && Form.Model != null) {
                Form.FormDescription = new FormDescription(Form.Description, Form.Model); // re-build description to add/remove UI elements.
            }
            StateHasChanged();
        }
        else {
            Console.WriteLine("  Not a list: " + items.GetType().ToString());
        }
    }

}
