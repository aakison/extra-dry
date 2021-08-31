using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class RuleEngineUpdateTreeAsyncTests {

        [Fact]
        public async Task IdentityUnchanged()
        {
            var services = new ServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var gcguid = Guid.NewGuid();
            var source = new Parent(guid, "Child", gcguid, "Grandchild");
            var destination = new Parent(guid, "Child", gcguid, "Grandchild");

            await rules.UpdateAsync(source, destination);

            Assert.Equal(guid, destination.Child.Uuid);
            Assert.Equal(gcguid, destination.Child.Grandchild.Uuid);
        }

        [Fact]
        public async Task ChildAddedWhenNotPresent()
        {
            var services = new ServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var source = new Parent { Child = new Child { Uuid = guid } };
            var destination = new Parent { Child = null };

            await rules.UpdateAsync(source, destination);

            Assert.NotNull(destination.Child);
            Assert.Equal(guid, destination.Child.Uuid);
        }

        //[Fact]
        //public async Task ChildCopyDoesntCopyIgnore()
        //{
        //    var services = new ServiceProviderStub();
        //    var rules = new RuleEngine(services);
        //    var guid = Guid.NewGuid();
        //    var gcguid = Guid.NewGuid();
        //    var source = new Parent(guid, "Child", gcguid, "Grandchild");
        //    var destination = new Parent(guid, "Child", gcguid, "Grandchild");
        //    source.Child.DontTouchThis = "dont-copy";
        //    destination.Child.DontTouchThis = "remains";

        //    await rules.UpdateAsync(source, destination);

        //    Assert.NotNull(destination.Child);
        //    Assert.Equal(guid, destination.Child.Uuid);
        //    Assert.Equal(gcguid, destination.Child.Grandchild.Uuid);
        //    Assert.Equal("remains", destination.Child.DontTouchThis);
        //}

        public class Grandchild {

            public Guid Uuid { get; set; } = Guid.NewGuid();

            public string Name { get; set; } = "Child";

        }

        public class Child {

            public Guid Uuid { get; set; } = Guid.NewGuid();

            public string Name { get; set; } = "Child";

            public Grandchild Grandchild { get; set; }

            [Rules(UpdateAction.Ignore)]
            public string DontTouchThis { get; set; }

            public override bool Equals(object obj) => (obj as Child)?.Uuid == Uuid;

            public override int GetHashCode() => Uuid.GetHashCode();

        }

        public class Parent {

            public Parent() { }

            public Parent(Guid childGuid, string childName, Guid grandchildGuid, string grandchildName)
            {
                Child = new Child {
                    Uuid = childGuid,
                    Name = childName,
                    Grandchild = new Grandchild {
                        Uuid = grandchildGuid,
                        Name = grandchildName,
                    }
                };
            }

            [Rules(UpdateAction.BlockChanges)]
            public int Id { get; set; } = 1;

            public Child Child { get; set; }

            [Rules(UpdateAction.AllowChanges)]
            public Child AllowChild { get; set; }

            //[Rules(UpdateAction.Ignore)]
            //public Child IgnoreChild { get; set; }

        }

        public class ChildEntityResolver : IEntityResolver<Child> {
            public Task<Child> ResolveAsync(Child exemplar)
            {
                if(database.ContainsKey(exemplar?.Uuid ?? Guid.Empty)) {
                    return Task.FromResult(database[exemplar.Uuid]);
                }
                else {
                    return Task.FromResult<Child>(null);
                }
            }

            public void AddChild(Child item)
            {
                database.Add(item.Uuid, item);
            }

            public Dictionary<Guid, Child> database = new Dictionary<Guid, Child>();

        }

        public class ServiceProviderStubWithChildResolver : IServiceProvider {
            public object GetService(Type serviceType)
            {
                if(serviceType.IsAssignableTo(typeof(IEntityResolver<Child>))) {
                    return ChildResolver;
                }
                else {
                    return null;
                }
            }

            public ChildEntityResolver ChildResolver { get; private set; } = new ChildEntityResolver();

        }

        public class ServiceProviderStub : IServiceProvider {
            public object GetService(Type serviceType) => null;

        }


    }
}
