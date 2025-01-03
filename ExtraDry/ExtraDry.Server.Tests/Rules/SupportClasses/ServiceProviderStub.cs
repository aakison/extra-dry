namespace ExtraDry.Server.Tests.Rules;

public class ServiceProviderStub : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        return null;
    }
}
