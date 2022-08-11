using ExtraDry.Blazor;
using ExtraDry.Core;
using Sample.Shared;

namespace Sample.Client;

public class EmployeeViewModel : ISubjectViewModel<Employee> {

    public string Caption(Employee employee) => $"{employee.FirstName} {employee.LastName} ({employee.Email})";

    public string Code(Employee _) => string.Empty;

    public string Title(Employee employee) => $"{employee.FirstName} {employee.LastName}";

    public string Subtitle(Employee employee) => employee.Email;

    public string Thumbnail(Employee employee) => Gravatar.ToGravatarUrl(employee.Email, 40);

    public string Description(Employee _) => string.Empty;

}
