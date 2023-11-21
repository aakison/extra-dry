namespace ExtraDry.Server.Tests.Rules;

public class RuleEngineCreateTests {
    [Fact]
    public async Task CreateRequiresItem()
    {
        var rules = new RuleEngine(new ServiceProviderStub());

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await rules.CreateAsync((object?)null));
    }

    [Fact]
    public async Task CreateWithValueTypesByDefault()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new Entity {
            DefaultInteger = 123,
            DefaultString = "Hello World",
            DefaultGuid = Guid.NewGuid(),
            DefaultState = State.Active
        };

        var entity = await rules.CreateAsync(exemplar);

        Assert.NotNull(entity);
        Assert.Equal(exemplar.DefaultInteger, entity.DefaultInteger);
        Assert.Equal(exemplar.DefaultString, entity.DefaultString);
        Assert.Equal(exemplar.DefaultGuid, entity.DefaultGuid);
        Assert.Equal(exemplar.DefaultState, entity.DefaultState);
    }

    [Fact]
    public async Task CreateWithValueTypes()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new Entity {
            Integer = 123,
            String = "Hello World",
            Uuid = Guid.NewGuid(),
            State = State.Active
        };

        var entity = await rules.CreateAsync(exemplar);

        Assert.NotNull(entity);
        Assert.Equal(exemplar.Integer, entity.Integer);
        Assert.Equal(exemplar.String, entity.String);
        Assert.Equal(exemplar.Uuid, entity.Uuid);
        Assert.Equal(exemplar.State, entity.State);
    }

    [Fact]
    public async Task CreateWithReferenceTypeByDefault()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new Entity {
            DefaultTestObject = new() {
                PropertyOne = "John Doe",
                PropertyTwo = "Jane Doe",
                PropertyThree = "Hello World"
            }
        };

        var entity = await rules.CreateAsync(exemplar);

        Assert.NotNull(entity);
        Assert.NotNull(entity.DefaultTestObject);
        Assert.NotEqual(exemplar.DefaultTestObject, entity.DefaultTestObject);
        Assert.Equal(exemplar.DefaultTestObject.PropertyOne, entity.DefaultTestObject?.PropertyOne);
        Assert.Equal(exemplar.DefaultTestObject.PropertyTwo, entity.DefaultTestObject?.PropertyTwo);
        Assert.Equal(exemplar.DefaultTestObject.PropertyThree, entity.DefaultTestObject?.PropertyThree);
    }

    [Fact]
    public async Task CreateWithReferenceType()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new Entity {
            TestObject = new() {
                PropertyOne = "John Doe",
                PropertyTwo = "Jane Doe",
                PropertyThree = "Hello World"
            }
        };

        var entity = await rules.CreateAsync(exemplar);

        Assert.NotNull(entity);
        Assert.NotNull(entity.TestObject);
        Assert.NotEqual(exemplar.TestObject, entity.TestObject);
        Assert.Equal(exemplar.TestObject.PropertyOne, entity.TestObject?.PropertyOne);
        Assert.Equal(exemplar.TestObject.PropertyTwo, entity.TestObject?.PropertyTwo);
        Assert.Equal(exemplar.TestObject.PropertyThree, entity.TestObject?.PropertyThree);
    }

    [Fact]
    public async Task CreateWithReferenceTypeByCreateDescendants()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new Entity {
            DesendantsTestObject = new() {
                TestObject = new()
            }
        };

        var entity = await rules.CreateAsync(exemplar);

        Assert.NotNull(entity);
        Assert.NotNull(entity.DesendantsTestObject);
        Assert.NotEqual(exemplar.DesendantsTestObject, entity.DesendantsTestObject);
        Assert.NotNull(entity.DesendantsTestObject?.TestObject);
        Assert.NotEqual(exemplar.DesendantsTestObject.TestObject, entity.DesendantsTestObject?.TestObject);
    }

    [Fact]
    public async Task CreateValidatableWithReferenceTypeByCreateDescendants()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new ValidatableEntity {
            DesendantsTestObject = new() {
                TestObject = new() { PropertyOne = "IgnoreThis" }
            }
        };

        var entity = await rules.CreateAsync(exemplar);

        Assert.NotNull(entity);
        Assert.NotNull(entity.DesendantsTestObject);
        Assert.NotEqual(exemplar.DesendantsTestObject, entity.DesendantsTestObject);
        Assert.NotNull(entity.DesendantsTestObject?.TestObject);
        Assert.NotEqual(exemplar.DesendantsTestObject.TestObject, entity.DesendantsTestObject?.TestObject);
    }

    [Fact]
    public async Task CreateWithIgnore()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new Entity {
            IgnoredProp = 123,
            IgnoredChild = new()
        };

        var valueTypes = await rules.CreateAsync(exemplar);

        Assert.NotNull(valueTypes);
        Assert.Equal(default, valueTypes.IgnoredProp);
        Assert.Equal(default, valueTypes.IgnoredChild);
    }

    [Theory]
    [InlineData(nameof(IgnoreDefaultsEntity.IntValue), 0)]
    [InlineData(nameof(IgnoreDefaultsEntity.NullableIntValue), null)]
    [InlineData(nameof(IgnoreDefaultsEntity.ObjectValue), null)]
    [InlineData(nameof(IgnoreDefaultsEntity.NullableObjectValue), null)]
    //[InlineData(nameof(IgnoreDefaultsEntity.GuidValue), Guid.Empty)] // not a literal... make a fact?
    [InlineData(nameof(IgnoreDefaultsEntity.NullableGuidValue), null)]
    [InlineData(nameof(IgnoreDefaultsEntity.StringValue), null)]
    [InlineData(nameof(IgnoreDefaultsEntity.NullableStringValue), null)]
    [InlineData(nameof(IgnoreDefaultsEntity.EnumValue), State.Unknown)] // enum value is 0
    [InlineData(nameof(IgnoreDefaultsEntity.NullableEnumValue), null)]
    public async Task CreateRuleDefaultsDontOverrideValues(string propertyName, object value)
    {
        var exemplar = new IgnoreDefaultsEntity();
        var property = exemplar.GetType().GetProperty(propertyName) ?? throw new ArgumentException("Missing property", nameof(propertyName));
        property.SetValue(exemplar, value);
        var rules = new RuleEngine(new ServiceProviderStub());

        var actual = await rules.CreateAsync(exemplar);

        var actualValue = property.GetValue(actual);
        Assert.NotEqual(value, actualValue);
    }

    [Theory]
    [InlineData(nameof(IgnoreDefaultsEntity.IntValue), 3)] // 0 is not the default...
    [InlineData(nameof(IgnoreDefaultsEntity.NullableIntValue), 0)] // 0 is not the default...
    [InlineData(nameof(IgnoreDefaultsEntity.StringValue), "abc")]
    [InlineData(nameof(IgnoreDefaultsEntity.NullableStringValue), "abc")]
    [InlineData(nameof(IgnoreDefaultsEntity.EnumValue), State.Another)]
    [InlineData(nameof(IgnoreDefaultsEntity.NullableEnumValue), State.Another)]
    public async Task CreateRuleValuesOverrideIgnoreDefaultValues(string propertyName, object value)
    {
        var exemplar = new IgnoreDefaultsEntity();
        var property = exemplar.GetType().GetProperty(propertyName) ?? throw new ArgumentException("Missing Property", nameof(propertyName));
        property.SetValue(exemplar, value);
        var rules = new RuleEngine(new ServiceProviderStub());

        var actual = await rules.CreateAsync(exemplar);

        var actualValue = property.GetValue(actual);
        Assert.Equal(value, actualValue);
    }

    [Fact]
    public async Task CreateWithJsonIgnore()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new Entity {
            JsonIgnoredProp = 123,
            JsonIgnoredChild = new()
        };

        var valueTypes = await rules.CreateAsync(exemplar);

        Assert.NotNull(valueTypes);
        Assert.Equal(default, valueTypes.JsonIgnoredProp);
        Assert.Equal(default, valueTypes.JsonIgnoredChild);
    }

    [Fact]
    public async Task CreateFailsOnExplicitValueTypePropertyBlock()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new BlockedPropertiesEntity { CreateBlockString = "abc" };

        await Assert.ThrowsAsync<DryException>(async () => await rules.CreateAsync(exemplar));
    }

    [Fact]
    public async Task CreateFailsOnImplicitValueTypePropertyBlock()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new BlockedPropertiesEntity { DefaultBlockString = "abc" };

        await Assert.ThrowsAsync<DryException>(async () => await rules.CreateAsync(exemplar));
    }

    // TODO: What does it mean to block an object?  
    // Do we recursively check all fields that are 'allow' and turn them to blocked?
    // Is it just at the object level?
    //[Fact]
    //public async Task CreateFailsOnExplicitObjectPropertyBlock()
    //{
    //    var rules = new RuleEngine(new ServiceProviderStub());
    //    var exemplar = new BlockedPropertiesEntity { BlockTestObject = new ChildEntity() };

    //    await Assert.ThrowsAsync<DryException>(async () => await rules.CreateAsync(exemplar));
    //}

}

public class IgnoreDefaultsEntity {

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public int IntValue { get; set; } = 1;

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public int? NullableIntValue { get; set; } = 1;

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public object ObjectValue { get; set; } = new();

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public object? NullableObjectValue { get; set; } = new();

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public Guid GuidValue { get; set; } = Guid.NewGuid();

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public Guid? NullableGuidValue { get; set; } = Guid.NewGuid();

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public string StringValue { get; set; } = "Hello World";

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public string? NullableStringValue { get; set; } = "Hello World";

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public State EnumValue { get; set; } = State.Active;

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public State? NullableEnumValue { get; set; } = State.Active;

}

public class Entity {
    
    public int DefaultInteger { get; set; }
    public string? DefaultString { get; set; }
    public Guid DefaultGuid { get; set; }
    public State DefaultState { get; set; }
    public ChildEntity? DefaultTestObject { get; set; }

    [Rules(CreateAction = RuleAction.Allow)]
    public int Integer { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public string? String { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public Guid Uuid { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public State State { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public ChildEntity? TestObject { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public ChildEntity? DesendantsTestObject { get; set; }

    [Rules(CreateAction = RuleAction.Ignore)]
    public int IgnoredProp { get; set; }
    
    [Rules(CreateAction = RuleAction.Ignore)]
    public ChildEntity? IgnoredChild { get; set; }

    [JsonIgnore]
    public int JsonIgnoredProp { get; set; }

    [JsonIgnore]
    public ChildEntity? JsonIgnoredChild { get; set; }
}

public class ValidatableEntity : IValidatableObject {

    [Rules(CreateAction = RuleAction.Allow)]
    public ChildEntity? DesendantsTestObject { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(DesendantsTestObject != null && !DesendantsTestObject.CreatedFromResourceReference && DesendantsTestObject.PropertyOne == "IgnoreThis") {
            yield return new ValidationResult($"The {nameof(DesendantsTestObject)} is not valid.", new[] { nameof(DesendantsTestObject) });
        }
    }
}


public class BlockedPropertiesEntity {

    [Rules(RuleAction.Block)]
    public string? DefaultBlockString { get; set; }
        
    [Rules(CreateAction = RuleAction.Block)]
    public string? CreateBlockString { get; set; }
        
    [Rules(CreateAction = RuleAction.Block)]
    public ChildEntity? BlockTestObject { get; set; }

}

public class ChildEntity {
    public string? PropertyOne { get; set; }
    public string? PropertyTwo { get; set; }
    public string? PropertyThree { get; set; }
    public ChildEntity? TestObject { get; set; }

    internal bool CreatedFromResourceReference => string.IsNullOrEmpty(PropertyTwo);
}

public class ChildEntityNoDefaultContructor {
    public ChildEntityNoDefaultContructor(string prop1)
    {
        PropertyOne = prop1;
    }
    public string PropertyOne { get; set; }
    public string? PropertyTwo { get; set; }
    public string? PropertyThree { get; set; }
    public ChildEntity? TestObject { get; set; }
}

internal sealed class InvalidReferenceTypes {
    public InvalidTestObject? TestObject { get; set; }
}

internal sealed class InvalidTestObject { }

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum State {
    Unknown = 0,
    Active = 1,
    Another = 2,
}
