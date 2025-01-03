using System.Text.Json;

namespace ExtraDry.Core.Tests.Rules;

public class RuleEngineUpdateDictionaryAsyncTests
{
    [Fact]
    public async Task IdentityUnchanged()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var guid = Guid.NewGuid();
        var source = new Target { };
        var destination = new Target { Uuid = guid };

        await rules.UpdateAsync(source, destination);

        Assert.Equal(guid, destination.Uuid);
    }

    [Fact]
    public async Task NullSourceLeavesNullDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        Assert.Null(source.NullableValues);

        await rules.UpdateAsync(source, destination);

        Assert.Null(destination.NullableValues);
    }

    [Fact]
    public async Task SourceCreatesDestinationWhenNull()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        source.NullableValues = [];

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.NullableValues);
    }

    [Theory]
    [InlineData("value")]
    [InlineData(123)]
    [InlineData(123.0)]
    [InlineData(123.0f)]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ValueCreatedWhenNotInDestination(object value)
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        source.Values.Add("key", value);

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values.Values);
        Assert.Equal(value, destination.Values["key"]);
    }

    [Fact]
    public async Task ValueReplacedWhenInDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        source.Values.Add("key", "value");
        destination.Values.Add("key", "old-value");

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values);
        Assert.Equal("value", destination.Values["key"]);
    }

    [Fact]
    public async Task MissingValueIgnoredWhenInDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        destination.Values.Add("key", "old-value");

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values);
        Assert.Equal("old-value", destination.Values["key"]);
    }

    [Fact]
    public async Task NullValueNoEffectWhenNotInDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        source.Values.Add("key", null);

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values);
        Assert.False(destination.Values.ContainsKey("key"));
    }

    [Fact]
    public async Task NullValueRemovesWhenInDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        source.Values.Add("key", null);
        destination.Values.Add("key", "old-value");

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values);
        Assert.False(destination.Values.ContainsKey("key"));
    }

    [Fact]
    public async Task IgnoreValueLeavesDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        source.IgnoredValues.Add("key", "value");
        destination.IgnoredValues.Add("key", "old-value");

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.IgnoredValues.Values);
        Assert.Equal("old-value", destination.IgnoredValues["key"]);
    }

    [Fact]
    public async Task ArraysAreNotAllowed()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        var list = new List<string> { "value1", "value2" };
        source.Values.Add("key", list);

        await Assert.ThrowsAsync<DryException>(
            async () => await rules.UpdateAsync(source, destination)
        );
    }

    [Fact]
    public async Task ComplexObjectsAreNotAllowed()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var source = new Target();
        var destination = new Target();
        var obj = new { subKey = "value1", anotherKey = "value2" };
        source.Values.Add("key", obj);

        await Assert.ThrowsAsync<DryException>(
            async () => await rules.UpdateAsync(source, destination)
        );
    }

    [Theory]
    [InlineData(@"""value""", "value")] // string
    [InlineData("123.45", 123.45)] // double as Numeric
    [InlineData("123", 123.0)] // int as Numeric
    [InlineData("true", true)] // bool as True
    [InlineData("false", false)] // bool as False
    public async Task JsonObjectTypeMapping(string element, object expected)
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var json = $"{{ \"Values\" : {{ \"key\" : {element} }} }}";
        var source = JsonSerializer.Deserialize<Target>(json);
        var destination = new Target();

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values);
        Assert.Equal(expected, destination.Values["key"]);
    }

    [Fact]
    public async Task JsonObjectNullTypeMapping()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services, new ExtraDryOptions());
        var json = "{ \"Values\" : { \"key\" : null } }";
        var source = JsonSerializer.Deserialize<Target>(json);
        var destination = new Target();

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values);
        Assert.False(destination.Values.ContainsKey("key"));
    }

    public class Target
    {
        [Rules(RuleAction.Block)]
        [JsonIgnore]
        public int Id { get; set; } = 1;

        [Rules(RuleAction.Ignore)]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public ExpandoValues Values { get; set; } = [];

        public ExpandoValues? NullableValues { get; set; }

        [Rules(RuleAction.Ignore)]
        public ExpandoValues IgnoredValues { get; set; } = [];
    }

    public class ServiceProviderStub : IServiceProvider
    {
        public object? GetService(Type serviceType) => null;
    }
}
