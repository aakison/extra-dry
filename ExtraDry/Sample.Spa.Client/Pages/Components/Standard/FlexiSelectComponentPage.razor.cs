using Microsoft.AspNetCore.Components;

namespace Sample.Spa.Client.Pages.Components.Standard;

public partial class FlexiSelectComponentPage 
    : ComponentBase, 
    ISubjectViewModel<Employee?>
{

    public string Code(Employee? _) => string.Empty;

    public string Title(Employee? employee) => $"{employee?.FirstName} {employee?.LastName}";

    public string Subtitle(Employee? employee) => employee?.Email ?? "no e-mail on file";

    public string Caption(Employee? employee) => $"{employee?.FirstName} {employee?.LastName} ({employee?.Email})";

    public string Icon(Employee? employee) => Gravatar.ToGravatarUrl(employee?.Email, 40);

    public string Description(Employee? _) => string.Empty;

    private string FormTitle { get; set; } = "Select";

    private bool ShowTitle { get; set; } = true;

    private bool MultiSelect { get; set; }

    private int ShowFilterThreshold { get; set; } = 10;

    private string FilterPlaceholder { get; set; } = "filter";

    private Employee? Value { get; set; }

    private List<Employee?>? Values { get; set; }

    private List<Employee> Data { get; set; } = [];

    protected override void OnInitialized()
    {
        Data.Add(new Employee { FirstName = "Adrian", LastName = "Akison", Email = "adrian@akison.com" });
    }

}
