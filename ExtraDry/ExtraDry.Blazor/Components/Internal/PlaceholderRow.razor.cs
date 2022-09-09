#nullable disable

namespace ExtraDry.Blazor.Components.Internal;

public partial class PlaceholderRow<T> : ComponentBase {

    [Parameter]
    public ViewModelDescription Description { get; set; }

    [Parameter]
    public int Height { get; set; } = 40;

    private Random Random { get; set; } = new Random();
}
