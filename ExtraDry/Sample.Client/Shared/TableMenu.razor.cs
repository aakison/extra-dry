#nullable enable

using ExtraDry.Blazor;
using ExtraDry.Blazor.Models;
using Microsoft.AspNetCore.Components;
using System;

namespace Sample.Client.Shared {

    public sealed partial class TableMenu : ComponentBase {

        [CascadingParameter]
        public MainLayout? Layout { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; } = null!;


        [Command(Icon = "plus")]
        public void AddItem() { }

        [Command(Icon = "filter")]
        public void Filter() { }


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


        public CommandInfo AddCommand => new CommandInfo(this, AddItem);
        public CommandInfo FilterCommand => new CommandInfo(this, Filter);
        public CommandInfo ExpandCommand => new CommandInfo(this, Expand) {
            IsVisible = () => !Layout?.ViewExpanded ?? false,
        };
        public CommandInfo CompressCommand => new CommandInfo(this, Compress) {
            IsVisible = () => Layout?.ViewExpanded ?? false,
        };

    }
}
