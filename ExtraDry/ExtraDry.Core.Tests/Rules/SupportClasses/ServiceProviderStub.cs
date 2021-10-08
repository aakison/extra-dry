using System;

namespace ExtraDry.Core.Tests.Rules {
    public class ServiceProviderStub : IServiceProvider {
        public object GetService(Type serviceType)
        {
            return null;
        }
    }
}
