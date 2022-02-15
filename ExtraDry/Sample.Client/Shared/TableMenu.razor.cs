#nullable enable

using ExtraDry.Blazor;
using ExtraDry.Blazor.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Sample.Client.Shared;

public sealed partial class TableMenu<TItem> : ComponentBase {

    [CascadingParameter]
    public MainLayout? Layout { get; set; }

    public IListService<TItem>? ItemsSource { get; set; }

    [Parameter]
    public Expandable Expandable { get; set; } = null!;

    [Command(Icon = "plus")]
    public void AddItem() { }

    [Command(Icon = "filter")]
    public async Task Filter()
    {
        await Expandable.Toggle();
    }

    [Command(Icon = "expand-alt")]
    public void Expand()
    {
        Console.WriteLine($"Expanding {Layout}");
        Layout?.Expand();
    }

    [Command(Icon = "compress-alt")]
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
        Icon = "times", Caption = "",
    };

}
