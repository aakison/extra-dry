using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class RuleEngineDeleteAsyncTests {

        [Fact]
        public async Task EntityFrameworkStyleDeleteSoftFailover()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };
            
            await rules.DeleteSoftAsync(item, () => items.Remove(item), async () => await SaveChangesAsync());

            Assert.Equal(SaveState.Done, state);
        }

        [Fact]
        public async Task EntityFrameworkStyleHardDelete()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            await rules.DeleteHardAsync(item, () => items.Remove(item), async () => await SaveChangesAsync());

            Assert.Equal(SaveState.Done, state);
        }

        private void SaveChanges()
        {
            state = SaveState.Processing;
            state = SaveState.Done;
        }

        private async Task SaveChangesAsync()
        {
            state = SaveState.Processing;
            await Task.Delay(1);
            state = SaveState.Done;
        }

        private SaveState state = SaveState.Pending;

        private enum SaveState {
            Pending = 0,
            Processing = 1,
            Done = 2,
        }

        public class ServiceProviderStub : IServiceProvider {
            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }
        }

        public class SoftDeletable {
            [Rules(DeleteValue = false)]
            public bool Active { get; set; } = true;
        }

    }
}
