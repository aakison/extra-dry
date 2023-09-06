using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Tests {
    public class ServiceProviderStub : IServiceProvider {
        public object? GetService(Type serviceType)
        {
            return null;
        }
    }
}
