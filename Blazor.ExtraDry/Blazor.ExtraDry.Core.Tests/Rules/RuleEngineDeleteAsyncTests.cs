using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class RuleEngineDeleteAsyncTests {

        [Fact]
        public void EntityFrameworkStyleDeleteExecutesSoft()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new SoftDeletable();
            var items = new List<SoftDeletable> { item };

            rules.Delete(item, () => items.Remove(item));

            Assert.NotEmpty(items);
            Assert.False(item.Active);
        }

        [Fact]
        public void EntityFrameworkStyleDeleteExecutesHard()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            rules.Delete(item, () => items.Remove(item));

            Assert.Empty(items);
        }

        [Fact]
        public async Task EntityFrameworkStyleDeleteSoftFailover()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };
            
            await rules.DeleteSoftAsync(item, () => items.Remove(item), async () => await SaveChangesAsync());

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
        }

        [Fact]
        public async Task EntityFrameworkStyleHardDelete()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            await rules.DeleteHardAsync(item, () => items.Remove(item), async () => await SaveChangesAsync());

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
        }

        [Fact]
        public void EntityFrameworkStyleHardDeleteSync()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            rules.DeleteHard(item, () => items.Remove(item), () => SaveChanges());

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
        }

        [Fact]
        public async Task EntityFrameworkStyleHardDeleteAsyncPrepare()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            await rules.DeleteHardAsync(item, 
                async () => {
                    await Task.Delay(1);
                    items.Remove(item);
                }, 
                () => SaveChanges()
            );

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
        }

        [Fact]
        public async Task EntityFrameworkStyleDeleteSoftAsyncPrepare()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            await rules.DeleteSoftAsync(item,
                async () => {
                    await Task.Delay(1);
                    items.Remove(item);
                },
                () => SaveChanges()
            );

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
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
