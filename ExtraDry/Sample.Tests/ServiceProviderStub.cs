namespace Sample.Tests
{
    public class ServiceProviderStub : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            return null;
        }
    }
}
