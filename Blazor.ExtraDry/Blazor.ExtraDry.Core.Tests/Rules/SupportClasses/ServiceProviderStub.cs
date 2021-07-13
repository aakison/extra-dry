using System;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class ServiceProviderStub : IServiceProvider {
        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
