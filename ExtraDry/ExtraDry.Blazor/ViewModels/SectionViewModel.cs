#nullable disable

using System.Collections.ObjectModel;

namespace ExtraDry.Blazor;

public class SectionViewModel {
    public string Name { get; set; }

    public string Path { get; set; }

    public string Icon { get; set; }

    public Collection<ArticleViewModel> Articles { get; } = [];
}
