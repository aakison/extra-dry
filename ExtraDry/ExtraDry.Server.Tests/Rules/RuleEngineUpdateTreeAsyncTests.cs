﻿namespace ExtraDry.Server.Tests.Rules;

public class RuleEngineUpdateTreeAsyncTests
{
    [Fact]
    public async Task IdentityUnchanged()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid = Guid.NewGuid();
        var gcguid = Guid.NewGuid();
        var source = new Parent(guid, "Child", gcguid, "Grandchild");
        var destination = new Parent(guid, "Child", gcguid, "Grandchild");

        await rules.UpdateAsync(source, destination);

        Assert.Equal(guid, destination.Child?.Uuid);
        Assert.Equal(gcguid, destination.Child?.Grandchild?.Uuid);
    }

    [Fact]
    public async Task ChildAddedWhenNotPresent()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid = Guid.NewGuid();
        var source = new Parent { Child = new Child { Uuid = guid } };
        var destination = new Parent { Child = null! };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Child);
        Assert.Equal(guid, destination.Child?.Uuid);
    }

    [Fact]
    public async Task ChildCopyDoesntCopyIgnore()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid = Guid.NewGuid();
        var gcguid = Guid.NewGuid();
        var source = new Parent(guid, "Child", gcguid, "Grandchild");
        var destination = new Parent(guid, "Child", gcguid, "Grandchild");
        source.Child.DontTouchThis = "dont-copy";
        destination.Child.DontTouchThis = "remains";

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Child);
        Assert.Equal(guid, destination.Child.Uuid);
        Assert.Equal(gcguid, destination.Child.Grandchild.Uuid);
        Assert.Equal("remains", destination.Child.DontTouchThis);
    }

    [Fact]
    public async Task ChildCopyExceptionOnBlock()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid = Guid.NewGuid();
        var gcguid = Guid.NewGuid();
        var source = new Parent(guid, "Child", gcguid, "Grandchild");
        var destination = new Parent(guid, "Child", gcguid, "Grandchild");
        source.Child.CantTouchThis = "dont-copy";
        destination.Child.CantTouchThis = "remains";

        await Assert.ThrowsAsync<DryException>(async () => await rules.UpdateAsync(source, destination));
    }

    [Fact]
    public async Task GrandchildCopyRecursion()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid = Guid.NewGuid();
        var gcguid = Guid.NewGuid();
        var source = new Parent(guid, "Child", gcguid, "Grandchild");
        var destination = new Parent(guid, "Child", gcguid, "Grandchild");
        source.Child.Grandchild.Name = "source";
        destination.Child.Grandchild.Name = "destination";

        await rules.UpdateAsync(source, destination);

        Assert.Equal("source", destination.Child.Grandchild.Name);
        Assert.NotEqual(source.Child.Grandchild, destination.Child.Grandchild);
    }

    [Fact]
    public async Task ProtectionFromTreeCycles()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var malformed = new Malformed();
        malformed.Child = malformed;
        var destination = new Malformed();

        await Assert.ThrowsAsync<DryException>(async () => await rules.UpdateAsync(malformed, destination));
    }

    [Fact]
    public async Task ExceptionOnTooDeepATree()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid = Guid.NewGuid();
        var gcguid = Guid.NewGuid();
        var source = new Parent(guid, "Child", gcguid, "Grandchild");
        var destination = new Parent(guid, "Child", gcguid, "Grandchild");
        source.Child.Grandchild.Name = "source";
        destination.Child.Grandchild.Name = "destination";

        rules.MaxRecursionDepth = 1;
        await Assert.ThrowsAsync<DryException>(async () => await rules.UpdateAsync(source, destination));
    }

    public class Grandchild
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = "Child";
    }

    public class Child
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = "Child";

        public Grandchild Grandchild { get; set; } = new();

        [Rules(RuleAction.Ignore)]
        public string DontTouchThis { get; set; } = string.Empty;

        [Rules(RuleAction.Block)]
        public string CantTouchThis { get; set; } = string.Empty;

        public override bool Equals(object? obj) => (obj as Child)?.Uuid == Uuid;

        public override int GetHashCode() => Uuid.GetHashCode();
    }

    public class Parent
    {
        public Parent()
        { }

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

        [Rules(RuleAction.Block)]
        [JsonIgnore]
        public int Id { get; set; } = 1;

        public Child Child { get; set; } = new();

        [Rules(RuleAction.Allow)]
        public Child? AllowChild { get; set; }

        [Rules(RuleAction.Ignore)]
        public Child? IgnoreChild { get; set; }
    }

    public class ChildEntityResolver : IEntityResolver<Child>
    {
        public Task<Child?> ResolveAsync(Child exemplar)
        {
            Database.TryGetValue(exemplar.Uuid, out var child);
            return Task.FromResult(child);
        }

        public void AddChild(Child item)
        {
            Database.Add(item.Uuid, item);
        }

        private Dictionary<Guid, Child> Database { get; set; } = [];
    }

    public class ServiceProviderStubWithChildResolver : IServiceProvider
    {
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

    public class ServiceProviderStub : IServiceProvider
    {
        public object? GetService(Type serviceType) => null;
    }

    public class Malformed
    {
        public string Name { get; set; } = "Name";

        public Malformed? Child { get; set; }
    }
}
