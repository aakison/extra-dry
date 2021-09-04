using ExtraDry.Core;
using ExtraDry.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ExtraDry.Core.Tests.Rules {
    public class RuleEngineDeleteAsyncTests {

        [Fact]
        public void EntityFrameworkStyleDeleteExecutesSoft()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new SoftDeletable();
            var items = new List<SoftDeletable> { item };

            var result = rules.Delete(item, () => items.Remove(item));

            Assert.NotEmpty(items);
            Assert.False(item.Active);
            Assert.Equal(DeleteResult.SoftDeleted, result);
        }

        [Fact]
        public void EntityFrameworkStyleDeleteExecutesHard()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            var result = rules.Delete(item, () => items.Remove(item));

            Assert.Empty(items);
            Assert.Equal(DeleteResult.HardDeleted, result);
        }

        [Fact]
        public async Task EntityFrameworkStyleDeleteSoftFailover()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };
            
            var result = await rules.DeleteSoftAsync(item, () => items.Remove(item), async () => await SaveChangesAsync());

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
            Assert.Equal(DeleteResult.HardDeleted, result);
        }

        [Fact]
        public async Task EntityFrameworkStyleHardDelete()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            var result = await rules.DeleteHardAsync(item, () => items.Remove(item), async () => await SaveChangesAsync());

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
            Assert.Equal(DeleteResult.HardDeleted, result);
        }

        [Fact]
        public void EntityFrameworkStyleHardDeleteSync()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            var result = rules.DeleteHard(item, () => items.Remove(item), () => SaveChanges());

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
            Assert.Equal(DeleteResult.HardDeleted, result);
        }

        [Fact]
        public async Task EntityFrameworkStyleHardDeleteAsyncPrepare()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            var result = await rules.DeleteHardAsync(item, 
                async () => {
                    await Task.Delay(1);
                    items.Remove(item);
                }, 
                () => SaveChanges()
            );

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
            Assert.Equal(DeleteResult.HardDeleted, result);
        }

        [Fact]
        public async Task EntityFrameworkStyleDeleteSoftAsyncPrepare()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var item = new object();
            var items = new List<object> { item };

            var result = await rules.DeleteSoftAsync(item,
                async () => {
                    await Task.Delay(1);
                    items.Remove(item);
                },
                () => SaveChanges()
            );

            Assert.Equal(SaveState.Done, state);
            Assert.Empty(items);
            Assert.Equal(DeleteResult.HardDeleted, result);
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

        public class SoftDeletable {
            [Rules(DeleteValue = false)]
            public bool Active { get; set; } = true;
        }

    }
}
