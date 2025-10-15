using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExtraDry.Core.Tests.Rules;

public class RuleEngineUpdateValueCollectionAsyncTests
{
    [Fact]
    public async Task GuidCollectionCopyToNull()
    {
        var services = new RuleEngineUpdateCollectionAsyncTests.ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var source = new ValueParent { Guids = [guid1, guid2] };
        var destination = new ValueParent { };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Guids);
        Assert.Equal(2, destination.Guids?.Count);
        Assert.Equal(guid1, destination.Guids?[0]);
        Assert.Equal(guid2, destination.Guids?[1]);
    }

    [Fact]
    public async Task GuidCollectionCopyToEmpty()
    {
        var services = new RuleEngineUpdateCollectionAsyncTests.ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var source = new ValueParent { Guids = [guid1, guid2] };
        var destination = new ValueParent { Guids = [] };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Guids);
        Assert.Equal(2, destination.Guids.Count);
        Assert.Equal(guid1, destination.Guids[0]);
        Assert.Equal(guid2, destination.Guids[1]);
    }

    [Fact]
    public async Task GuidCollectionCopyRemovesExtras()
    {
        var services = new RuleEngineUpdateCollectionAsyncTests.ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid = Guid.NewGuid();
        var source = new ValueParent { Guids = [guid] };
        var destination = new ValueParent { Guids = [Guid.NewGuid(), guid, Guid.NewGuid()] };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Guids);
        Assert.Single(destination.Guids);
        Assert.Equal(guid, destination.Guids[0]);
    }

    [Fact]
    public async Task IntCollectionCopyToNull()
    {
        var services = new RuleEngineUpdateCollectionAsyncTests.ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new ValueParent { Ints = [1, 2, 3] };
        var destination = new ValueParent { };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Ints);
        Assert.Equal(3, destination.Ints?.Count);
        Assert.Equal(1, destination.Ints?[0]);
        Assert.Equal(2, destination.Ints?[1]);
        Assert.Equal(3, destination.Ints?[2]);
    }

    [Fact]
    public async Task IntCollectionCopyRemovesExtras()
    {
        var services = new RuleEngineUpdateCollectionAsyncTests.ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new ValueParent { Ints = [42] };
        var destination = new ValueParent { Ints = [1, 42, 99] };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Ints);
        Assert.Single(destination.Ints);
        Assert.Equal(42, destination.Ints[0]);
    }

    [Fact]
    public async Task StringCollectionCopyToNull()
    {
        var services = new RuleEngineUpdateCollectionAsyncTests.ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new ValueParent { Strings = ["a", "b", "c"] };
        var destination = new ValueParent { };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Strings);
        Assert.Equal(3, destination.Strings?.Count);
        Assert.Equal("a", destination.Strings?[0]);
        Assert.Equal("b", destination.Strings?[1]);
        Assert.Equal("c", destination.Strings?[2]);
    }

    [Fact]
    public async Task StringCollectionCopyRemovesExtras()
    {
        var services = new RuleEngineUpdateCollectionAsyncTests.ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new ValueParent { Strings = ["keep"] };
        var destination = new ValueParent { Strings = ["remove", "keep", "remove2"] };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Strings);
        Assert.Single(destination.Strings);
        Assert.Equal("keep", destination.Strings[0]);
    }

    [Fact]
    public async Task ValueCollectionsCopyNullClearsCollection()
    {
        var services = new RuleEngineUpdateCollectionAsyncTests.ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new ValueParent { };
        var destination = new ValueParent {
            Guids = [Guid.NewGuid()],
            Ints = [1],
            Strings = ["a"]
        };

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Guids);
        Assert.Empty(destination.Guids);
        Assert.NotNull(destination.Ints);
        Assert.Empty(destination.Ints);
        Assert.NotNull(destination.Strings);
        Assert.Empty(destination.Strings);
    }

    public class ValueParent : IValidatableObject
    {
        public List<Guid>? Guids { get; set; }
        public List<int>? Ints { get; set; }
        public List<string>? Strings { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => [];
    }
}
