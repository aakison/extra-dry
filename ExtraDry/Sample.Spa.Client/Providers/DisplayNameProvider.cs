namespace Sample.Spa.Client;

public class DisplayNameProvider(CrudClient<Employee> employeeService) : IDisplayNameProvider
{
    public async Task<string> ResolveDisplayNameAsync(string user)
    {
        try {
            var employee = await employeeService.ReadAsync(user);
            return employee?.FirstName ?? "";
        }
        catch(Exception) {
            return "";
        }
    }
}
