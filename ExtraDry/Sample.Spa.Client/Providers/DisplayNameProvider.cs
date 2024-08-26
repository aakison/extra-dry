namespace Sample.Spa.Client;

public class DisplayNameProvider(CrudService<Employee> employeeService) : IDisplayNameProvider
{
    public async Task<string> ResolveDisplayNameAsync(string user)
    {
        try {
            var employee = await employeeService.RetrieveAsync(user);
            return employee?.FirstName ?? "";
        }
        catch(Exception) {
            return "";
        }
    }
}
