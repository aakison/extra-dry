namespace ExtraDry.Core.Tests.Rules;

public class RuleEngineUpdateCollectionAsyncTests {

    [Fact]
    public async Task IdentityUnchanged()
    {
        var services = new ServiceProviderStub();
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
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var guid = Guid.NewGuid();
        var source = new Parent { Child = new Child { Uuid = guid } };
        var destination = new Parent { Child = null };

        await rules.UpdateAsync(source, destination);

        Assert.Null(destination.Children);
        Assert.NotNull(destination.Child);
        Assert.Equal(guid, destination.Child?.Uuid);
    }

    [Fact]
    public async Task ChildReplacesWhenPresent()
    {
        var services = new ServiceProviderStub();
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
        var services = new ServiceProviderStubWithChildResolver();
        var rules = new RuleEngine(services);
        var guid = Guid.NewGuid();
        var databaseMatch = new Child { Uuid = guid, Name = "InDatabase" };
        services.ChildResolver.AddChild(databaseMatch);
        var source = new Parent { Child = new Child { Uuid = guid, Name = "IgnoreMe" } };
        var destination = new Parent { Child = new Child { Uuid = Guid.NewGuid(), Name = "PreviousChild" } };

        await rules.UpdateAsync(source, destination);

        Assert.Null(destination.Children);
        Assert.NotNull(destination.Child);
        Assert.Equal(guid, destination.Child.Uuid);
        Assert.Equal("InDatabase", destination.Child.Name);
    }

    [Fact]
    public async Task ChildrenCopyToNull()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Parent { Children = new List<Child> {
            new Child { Uuid = Guid.NewGuid(), Name = "Child1" },
            new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
        }};
        var destination = new Parent { };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Children);
        Assert.Equal(2, destination.Children?.Count);
        Assert.Equal("Child1", destination.Children?[0].Name);
        Assert.Equal(source.Children[0].Uuid, destination.Children?[0].Uuid);
        Assert.Equal("Child2", destination.Children?[1].Name);
        Assert.Equal(source.Children[1].Uuid, destination.Children?[1].Uuid);
    }

    [Fact]
    public async Task ChildrenCopyToEmpty()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
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
        var services = new ServiceProviderStub();
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
        var services = new ServiceProviderStub();
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
        var services = new ServiceProviderStub();
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
        var services = new ServiceProviderStubWithChildResolver();
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
        var services = new ServiceProviderStubWithChildResolver();
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

    [Fact]
    public async Task IgnoreChildrenDoesNothing()
    {
        var services = new ServiceProviderStubWithChildResolver();
        var rules = new RuleEngine(services);
        var source = new Parent {
            IgnoredChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child1" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
            }
        };
        var destination = new Parent {
            IgnoredChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child3" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child4" },
            }
        };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.IgnoredChildren);
        Assert.Equal(2, destination.IgnoredChildren.Count);
        Assert.Equal("Child3", destination.IgnoredChildren[0].Name);
        Assert.Equal("Child4", destination.IgnoredChildren[1].Name);
    }

    [Fact]
    public async Task IgnoreDefaultsChildrenReplaces()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Parent {
            IgnoredDefaultsChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child1" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
            }
        };
        var destination = new Parent {
            IgnoredDefaultsChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child3" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child4" },
            }
        };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.IgnoredDefaultsChildren);
        Assert.Equal(2, destination.IgnoredDefaultsChildren.Count);
        Assert.Equal("Child1", destination.IgnoredDefaultsChildren[0].Name);
        Assert.Equal("Child2", destination.IgnoredDefaultsChildren[1].Name);
    }


    [Fact]
    public async Task IgnoreDefaultsChildrenIgnoresNull()
    {
        var services = new ServiceProviderStubWithChildResolver();
        var rules = new RuleEngine(services);
        var source = new Parent {
            IgnoredDefaultsChildren = null
        };
        var destination = new Parent {
            IgnoredDefaultsChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child3" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child4" },
            }
        };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.IgnoredDefaultsChildren);
        Assert.Equal(2, destination.IgnoredDefaultsChildren.Count);
        Assert.Equal("Child3", destination.IgnoredDefaultsChildren[0].Name);
        Assert.Equal("Child4", destination.IgnoredDefaultsChildren[1].Name);
    }

    [Fact]
    public async Task IgnoreDefaultsChildrenEmptysWhenCollectionEmpty()
    {
        var services = new ServiceProviderStubWithChildResolver();
        var rules = new RuleEngine(services);
        var source = new Parent {
            IgnoredDefaultsChildren = new List<Child>(),
        };
        var destination = new Parent {
            IgnoredDefaultsChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child3" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child4" },
            }
        };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.IgnoredDefaultsChildren);
        Assert.Empty(destination.IgnoredDefaultsChildren);
    }

    [Fact]
    public async Task BlockChangesCollectionsBothEmptyOk()
    {
        var services = new ServiceProviderStubWithChildResolver();
        var rules = new RuleEngine(services);
        var source = new Parent {
            BlockedChildren = new List<Child>(),
        };
        var destination = new Parent {
            BlockedChildren = new List<Child>(),
        };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.BlockedChildren);
        Assert.Empty(destination.BlockedChildren);
    }

    [Fact]
    public async Task BlockChangesCollectionsBothNullOk()
    {
        var services = new ServiceProviderStubWithChildResolver();
        var rules = new RuleEngine(services);
        var source = new Parent {
            BlockedChildren = null,
        };
        var destination = new Parent {
            BlockedChildren = null,
        };

        await rules.UpdateAsync(source, destination);

        Assert.Null(destination.BlockedChildren);
    }

    [Fact]
    public async Task BlockChangesCollectionsBothSameOk()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var source = new Parent {
            BlockedChildren = new List<Child> {
                new Child { Uuid = guid1, Name = "Child3" },
                new Child { Uuid = guid2, Name = "Child4" },
            }
        };
        var destination = new Parent {
            BlockedChildren = new List<Child> {
                new Child { Uuid = guid1, Name = "Child3" },
                new Child { Uuid = guid2, Name = "Child4" },
            }
        };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.BlockedChildren);
        Assert.Equal(2, destination.BlockedChildren.Count);
        Assert.Equal("Child3", destination.BlockedChildren[0].Name);
        Assert.Equal("Child4", destination.BlockedChildren[1].Name);
    }


    [Fact]
    public async Task BlockChangesEmptyOverwriteThrows()
    {
        var services = new ServiceProviderStubWithChildResolver();
        var rules = new RuleEngine(services);
        var source = new Parent {
            BlockedChildren = new List<Child>()
        };
        var destination = new Parent {
            BlockedChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child3" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child4" },
            }
        };

        await Assert.ThrowsAsync<DryException>(async () => await rules.UpdateAsync(source, destination));
    }

    [Fact]
    public async Task BlockChangesNullOverwriteThrows()
    {
        var services = new ServiceProviderStubWithChildResolver();
        var rules = new RuleEngine(services);
        var source = new Parent {
            BlockedChildren = null
        };
        var destination = new Parent {
            BlockedChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child3" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child4" },
            }
        };

        await Assert.ThrowsAsync<DryException>(async () => await rules.UpdateAsync(source, destination));
    }

    [Fact]
    public async Task BlockChangesDifferentSetOverwriteThrows()
    {
        var services = new ServiceProviderStubWithChildResolver();
        var rules = new RuleEngine(services);
        var source = new Parent {
            BlockedChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child1" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child2" },
            }
        };
        var destination = new Parent {
            BlockedChildren = new List<Child> {
                new Child { Uuid = Guid.NewGuid(), Name = "Child3" },
                new Child { Uuid = Guid.NewGuid(), Name = "Child4" },
            }
        };

        await Assert.ThrowsAsync<DryException>(async () => await rules.UpdateAsync(source, destination));
    }

    public class Child {

        public Guid Uuid { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = "Child";

        public override bool Equals(object? obj) => (obj as Child)?.Uuid == Uuid;

        public override int GetHashCode() => Uuid.GetHashCode();

        // This is used to determine if this was created from the
        // ResourceReferenceConverter by comparing the default property values.
        // This can then be used to determine if validation can be run against it.
        internal bool CreatedFromResourceReference => Name == "Child";

    }

    public class Parent : IValidatableObject {

        [Rules(RuleAction.Block)]
        [JsonIgnore]
        public int Id { get; set; } = 1;

        public Child? Child { get; set; }

        public List<Child>? Children { get; set; }

        [Rules(RuleAction.Ignore)]
        public List<Child>? IgnoredChildren { get; set; }

        [Rules(RuleAction.IgnoreDefaults)]
        public List<Child>? IgnoredDefaultsChildren { get; set; }

        [Rules(RuleAction.Block)]
        public List<Child>? BlockedChildren { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Child != default && !Child.CreatedFromResourceReference && Child.Name == "PreviousChild") {
                yield return new ValidationResult($"The {nameof(Child)} is not valid.", new[] { nameof(Child) });
            }
        }
    }

    public class ChildEntityResolver : IEntityResolver<Child> {
        public Task<Child?> ResolveAsync(Child exemplar)
        {
            if(database.ContainsKey(exemplar.Uuid)) {
                return Task.FromResult((Child?)database[exemplar.Uuid]);
            }
            else {
                return Task.FromResult<Child?>(null);
            }
        }

        public void AddChild(Child item)
        {
            database.Add(item.Uuid, item);
        }

        public Dictionary<Guid, Child> database = new();

    }

    public class ServiceProviderStubWithChildResolver : IServiceProvider {
        public object? GetService(Type serviceType)
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
        public object? GetService(Type serviceType) => null;

    }

}
