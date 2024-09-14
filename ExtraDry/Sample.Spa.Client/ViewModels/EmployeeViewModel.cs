using ExtraDry.Blazor;
using Sample.Shared;

namespace Sample.Spa.Client;

public class EmployeeViewModel : ISubjectViewModel<Employee> {

    public string Caption(Employee item) => $"{item.FirstName} {item.LastName} ({item.Email})";

    public string Code(Employee item) => string.Empty;

    public string Title(Employee item) => $"{item.FirstName} {item.LastName}";

    public string Subtitle(Employee item) => item.Email ?? string.Empty;

    public string Icon(Employee item) => Gravatar.ToGravatarUrl(item.Email, 40);

    public string Description(Employee item) => string.Empty;

}
