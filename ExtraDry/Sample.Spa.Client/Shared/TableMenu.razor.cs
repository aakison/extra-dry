#nullable enable

using ExtraDry.Blazor;
using ExtraDry.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Sample.Spa.Client.Shared;

public sealed partial class TableMenu<TItem> : ComponentBase {

    [CascadingParameter]
    public MainLayout? Layout { get; set; }

    public IListService<TItem>? ItemsSource { get; set; }

    [Parameter]
    public Reveal Reveal { get; set; } = null!;

    [Command(Icon = "plus")]
    public void AddItem() { }

    [Command(Icon = "filter")]
    public async Task Filter()
    {
        await Reveal.ToggleAsync();
    }

    [Command(Icon = "full-screen")]
    public void Expand()
    {
        Console.WriteLine($"Expanding {Layout}");
        Layout?.Expand();
    }

    [Command(Icon = "windowed-screen")]
    public void Compress()
    {
        Console.WriteLine($"Compress {Layout}");
        Layout?.Compress();
    }


    public CommandInfo AddCommand => new(this, AddItem);
    public CommandInfo FilterCommand => new(this, Filter);
    public CommandInfo ExpandCommand => new(this, Expand) {
        IsVisible = () => !Layout?.ViewExpanded ?? false,
    };
    public CommandInfo CompressCommand => new(this, Compress) {
        IsVisible = () => Layout?.ViewExpanded ?? false,
    };
    public CommandInfo CloseFilterCommand => new(this, Filter) {
        Icon = "close-dialog", Caption = "",
    };

}
