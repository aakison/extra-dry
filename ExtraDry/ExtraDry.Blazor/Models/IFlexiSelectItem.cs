#nullable enable

namespace ExtraDry.Blazor.Models;

public interface IFlexiSelectItem {

    public string CssClass { get; set; }

    public string Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Thumbnail { get; set; }

}
