using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor;

public class AppViewModel {

    public string Name { get; set; }

    public string Copyright { get; set; }


    [SuppressMessage("Security", "DRY1304:Properties that might leak PID should be JsonIgnore.", Justification = "Not an auditing verison, the application version.")]
    public string Version { get; set; }

    public Collection<SectionViewModel> Sections { get; } = new Collection<SectionViewModel>();
}
