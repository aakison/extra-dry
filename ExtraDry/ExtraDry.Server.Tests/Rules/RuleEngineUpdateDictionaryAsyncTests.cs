namespace ExtraDry.Core.Tests.Rules;

public class RuleEngineUpdateDictionaryAsyncTests {

    [Fact]
    public async Task IdentityUnchanged()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
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
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        Assert.Null(source.NullableValues.Values);

        await rules.UpdateAsync(source, destination);

        Assert.Null(destination.NullableValues.Values);
    }

    [Fact]
    public async Task SourceCreatesDestinationWhenNull()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        source.NullableValues.Values = new();

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.NullableValues.Values);
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
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        source.Values.Values.Add("key", value);

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values.Values);
        Assert.Equal(value, destination.Values.Values["key"]);
    }

    [Fact]
    public async Task ValueReplacedWhenInDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        source.Values.Values.Add("key", "value");
        destination.Values.Values.Add("key", "old-value");

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values.Values);
        Assert.Equal("value", destination.Values.Values["key"]);
    }

    [Fact]
    public async Task MissingValueIgnoredWhenInDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        destination.Values.Values.Add("key", "old-value");

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values.Values);
        Assert.Equal("old-value", destination.Values.Values["key"]);
    }

#pragma warning disable CS8625 
    [Fact]
    public async Task NullValueNoEffectWhenNotInDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        source.Values.Values.Add("key", null);

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values.Values);
        Assert.False(destination.Values.Values.ContainsKey("key"));
    }
#pragma warning restore CS8625


#pragma warning disable CS8625
    [Fact]
    public async Task NullValueRemovesWhenInDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        source.Values.Values.Add("key", null);
        destination.Values.Values.Add("key", "old-value");

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.Values.Values);
        Assert.False(destination.Values.Values.ContainsKey("key"));
    }
#pragma warning restore CS8625

    [Fact]
    public async Task IgnoreValueLeavesDestination()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        source.IgnoredValues.Values.Add("key", "value");
        destination.IgnoredValues.Values.Add("key", "old-value");

        await rules.UpdateAsync(source, destination);

        Assert.NotNull(destination.IgnoredValues.Values);
        Assert.Equal("old-value", destination.IgnoredValues.Values["key"]);
    }

    [Fact]
    public async Task ArraysAreNotAllowed()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        var list = new List<string> { "value1", "value2" };
        source.Values.Values.Add("key", list);

        await Assert.ThrowsAsync<DryException>(
            async () => await rules.UpdateAsync(source, destination)
        );
    }

    [Fact]
    public async Task ComplexObjectsAreNotAllowed()
    {
        var services = new ServiceProviderStub();
        var rules = new RuleEngine(services);
        var source = new Target();
        var destination = new Target();
        var obj = new { subKey = "value1", anotherKey = "value2" };
        source.Values.Values.Add("key", obj);

        await Assert.ThrowsAsync<DryException>(
            async () => await rules.UpdateAsync(source, destination)
        );
    }

    public class Target {

        [Rules(RuleAction.Block)]
        [JsonIgnore]
        public int Id { get; set; } = 1;

        [Rules(RuleAction.Ignore)]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public ExpandoValues Values { get; set; } = new();

        public NullableExpandoValues NullableValues { get; set; } = new();

        public IgnoredExpandoValues IgnoredValues { get; set; } = new();

    }

    public class ExpandoValues {
        public Dictionary<string, object> Values { get; set; } = new();
    }

    public class NullableExpandoValues
    {
        public Dictionary<string, object>? Values { get; set; }
    }

    public class IgnoredExpandoValues
    {
        [Rules(RuleAction.Ignore)]
        public Dictionary<string, object> Values { get; set; } = new();
    }

    public class ServiceProviderStub : IServiceProvider {
        public object? GetService(Type serviceType) => null;
    }

}
