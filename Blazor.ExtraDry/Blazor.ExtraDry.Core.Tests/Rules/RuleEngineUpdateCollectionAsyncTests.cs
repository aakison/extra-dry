using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class RuleEngineUpdateAsyncTests {

        [Fact]
        public async Task IdentityUnchanged()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var source = new Parent { Child = new Child { Uuid = guid } };
            var destination = new Parent { Child = new Child { Uuid = guid } };

            await rules.UpdateAsync(source, destination);

            Assert.Null(destination.Children);
            Assert.Equal(guid, destination.Child.Uuid);
        }

        [Fact]
        public async Task ChildAddedWhenNotPresent()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var source = new Parent { Child = new Child { Uuid = guid } };
            var destination = new Parent { Child = null };

            await rules.UpdateAsync(source, destination);

            Assert.Null(destination.Children);
            Assert.NotNull(destination.Child);
            Assert.Equal(guid, destination.Child.Uuid);
        }

        [Fact]
        public async Task ChildReplacesWhenPresent()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var source = new Parent { Child = new Child { Uuid = guid } };
            var destination = new Parent { Child = new Child { Uuid = Guid.NewGuid() } };

            await rules.UpdateAsync(source, destination);

            Assert.Null(destination.Children);
            Assert.NotNull(destination.Child);
            Assert.Equal(guid, destination.Child.Uuid);
        }

        [Fact]
        public async Task ChildMatchesFromDatabase()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var databaseMatch = new Child { Uuid = guid, Name = "InDatabase" };
            services.ChildResolver.AddChild(databaseMatch);
            var source = new Parent { Child = new Child { Uuid = guid, Name = "IgnoreMe" } };
            var destination = new Parent { Child = new Child { Uuid = Guid.NewGuid() } };

            await rules.UpdateAsync(source, destination);

            Assert.Null(destination.Children);
            Assert.NotNull(destination.Child);
            Assert.Equal(guid, destination.Child.Uuid);
            Assert.Equal("InDatabase", destination.Child.Name);
        }

        [Fact]
        public async Task ChildrenCopyToNull()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var databaseMatch = new Child { Uuid = guid, Name = "InDatabase" };
            services.ChildResolver.AddChild(databaseMatch);
            var source = new Parent { Children = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child1" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
            }};
            var destination = new Parent { };

            await rules.UpdateAsync(source, destination);

            Assert.NotNull(destination.Children);
            Assert.Equal(2, destination.Children.Count);
            Assert.Equal("Child1", destination.Children[0].Name);
            Assert.Equal(source.Children[0].Uuid, destination.Children[0].Uuid);
            Assert.Equal("Child2", destination.Children[1].Name);
            Assert.Equal(source.Children[1].Uuid, destination.Children[1].Uuid);
        }

        [Fact]
        public async Task ChildrenCopyToEmpty()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var databaseMatch = new Child { Uuid = guid, Name = "InDatabase" };
            services.ChildResolver.AddChild(databaseMatch);
            var source = new Parent {
                Children = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child1" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
            }
            };
            var destination = new Parent { Children = new List<Child>() };

            await rules.UpdateAsync(source, destination);

            Assert.NotNull(destination.Children);
            Assert.Equal(2, destination.Children.Count);
            Assert.Equal("Child1", destination.Children[0].Name);
            Assert.Equal(source.Children[0].Uuid, destination.Children[0].Uuid);
            Assert.Equal("Child2", destination.Children[1].Name);
            Assert.Equal(source.Children[1].Uuid, destination.Children[1].Uuid);
        }

        [Fact]
        public async Task ChildrenCopyOverwritesExisting()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var source = new Parent {
                Children = new List<Child> {
                    new Child { Uuid = Guid.NewGuid(), Name = "Child3" },
                    new Child { Uuid = Guid.NewGuid(), Name = "Child4" },
                }
            };
            var destination = new Parent {
                Children = new List<Child> {
                    new Child { Uuid = Guid.NewGuid(), Name = "Child1" },
                    new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
                }
            };

            await rules.UpdateAsync(source, destination);

            Assert.NotNull(destination.Children);
            Assert.Equal(2, destination.Children.Count);
            Assert.Equal("Child3", destination.Children[0].Name);
            Assert.Equal(source.Children[0].Uuid, destination.Children[0].Uuid);
            Assert.Equal("Child4", destination.Children[1].Name);
            Assert.Equal(source.Children[1].Uuid, destination.Children[1].Uuid);
        }

        [Fact]
        public async Task ChildrenCopyRemovesExtras()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var source = new Parent {
                Children = new List<Child> {
                    new Child { Uuid = guid, Name = "Child1" },
                }
            };
            var destination = new Parent {
                Children = new List<Child> {
                    new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
                    new Child { Uuid = guid, Name = "Child1" },
                    new Child { Uuid = Guid.NewGuid(), Name = "Child3" },
                }
            };

            await rules.UpdateAsync(source, destination);

            Assert.NotNull(destination.Children);
            Assert.Single(destination.Children);
            Assert.Equal("Child1", destination.Children[0].Name);
            Assert.Equal(source.Children[0].Uuid, destination.Children[0].Uuid);
        }

        [Fact]
        public async Task ChildrenCopyAddsExtra()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var guid = Guid.NewGuid();
            var source = new Parent {
                Children = new List<Child> {
                    new Child { Uuid = guid, Name = "Child New Name" },
                    new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
                }
            };
            var destination = new Parent {
                Children = new List<Child> {
                    new Child { Uuid = guid, Name = "Child Original Name" },
                }
            };
            var originalObject = destination.Children.First();

            await rules.UpdateAsync(source, destination);

            Assert.NotNull(destination.Children);
            Assert.Equal("Child Original Name", destination.Children[0].Name);
            Assert.Equal(source.Children[0].Uuid, destination.Children[0].Uuid);
            Assert.Equal("Child2", destination.Children[1].Name);
            Assert.Equal(originalObject, destination.Children[0]);  // ensure not just an overwrite, keep original object.
        }

        [Fact]
        public async Task ChildrenCopyNullClearsCollection()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var source = new Parent { };
            var destination = new Parent {
                Children = new List<Child> {
                    new Child { Uuid = Guid.NewGuid(), Name = "Child1" },
                    new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
                }
            };

            await rules.UpdateAsync(source, destination);

            Assert.NotNull(destination.Children);
            Assert.Empty(destination.Children);
        }

        [Fact]
        public async Task ChildrenCopyEmptyClearsCollection()
        {
            var services = new LocalServiceProviderStub();
            var rules = new RuleEngine(services);
            var source = new Parent { Children = new List<Child>() };
            var destination = new Parent {
                Children = new List<Child> {
                    new Child { Uuid = Guid.NewGuid(), Name = "Child1" },
                    new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
                }
            };

            await rules.UpdateAsync(source, destination);

            Assert.NotNull(destination.Children);
            Assert.Empty(destination.Children);
        }

        public class Child {

            public Guid Uuid { get; set; } = Guid.NewGuid();

            public string Name { get; set; } = "Child";

            public override bool Equals(object obj) => (obj as Child)?.Uuid == Uuid;

            public override int GetHashCode() => Uuid.GetHashCode();

        }

        public class Parent {

            [Rules(UpdateAction.BlockChanges)]
            public int Id { get; set; } = 1;

            public Child Child { get; set; }

            public List<Child> Children { get; set; }

        }

        private Parent SampleParent() => new Parent {
            Child = new Child(),
            Children = {
                new Child(),
                new Child(),
            }
        };

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

        public class LocalServiceProviderStub : IServiceProvider {
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


    }
}
