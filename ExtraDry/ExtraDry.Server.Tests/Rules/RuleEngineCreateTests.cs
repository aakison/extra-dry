namespace ExtraDry.Core.Tests.Rules;

public class RuleEngineCreateTests {
    [Fact]
    public async Task CreateRequiresItem()
    {
        var rules = new RuleEngine(new ServiceProviderStub());

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await rules.CreateAsync((object)null));
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
            Guid = Guid.NewGuid(),
            State = State.Active
        };

        var entity = await rules.CreateAsync(exemplar);

        Assert.NotNull(entity);
        Assert.Equal(exemplar.Integer, entity.Integer);
        Assert.Equal(exemplar.String, entity.String);
        Assert.Equal(exemplar.Guid, entity.Guid);
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
        Assert.Equal(exemplar.DefaultTestObject.PropertyOne, entity.DefaultTestObject.PropertyOne);
        Assert.Equal(exemplar.DefaultTestObject.PropertyTwo, entity.DefaultTestObject.PropertyTwo);
        Assert.Equal(exemplar.DefaultTestObject.PropertyThree, entity.DefaultTestObject.PropertyThree);
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
        Assert.Equal(exemplar.TestObject.PropertyOne, entity.TestObject.PropertyOne);
        Assert.Equal(exemplar.TestObject.PropertyTwo, entity.TestObject.PropertyTwo);
        Assert.Equal(exemplar.TestObject.PropertyThree, entity.TestObject.PropertyThree);
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
        Assert.NotNull(entity.DesendantsTestObject.TestObject);
        Assert.NotEqual(exemplar.DesendantsTestObject.TestObject, entity.DesendantsTestObject.TestObject);
    }

    // TODO: what is this testing?
    //[Fact]
    // public async Task CreateWithInvalidReferenceType()
    //{
    //    var rules = new RuleEngine(new ServiceProviderStub());
    //    var exemplar = new InvalidReferenceTypes {
    //        TestObject = new()
    //    };

    //    var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await rules.CreateAsync(exemplar));

    //    Assert.NotNull(exception);
    //    Assert.Equal("Attempt to create private or nested type 'InvalidTestObject'", exception.Message);
    //}

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

    [Fact]
    public async Task CreateWithIgnoreDefault()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var exemplar = new Entity {
            IgnoredDefaultProp = Guid.Empty,
            IgnoredDefaultNullableProp = null,
            IgnoredDefaultGuidPropWithInit = Guid.Empty,
            IgnoredDefaultGuidNullablePropWithInit = Guid.Empty,
            IgnoredDefaultStringPropWithInit = null,
            IgnoredDefaultIntPropWithInit = 0,
            IgnoredDefaultEnumPropWithInit = State.Unknowm,
            IgnoredDefaultChild = null,
            IgnoredDefaultChildWithInit = null,
            IgnoredDefaultChild1WithInit = null
        };

        var valueTypes = await rules.CreateAsync(exemplar);

        Assert.NotNull(valueTypes);
        Assert.Equal(default, valueTypes.IgnoredDefaultProp);
        Assert.Equal(default, valueTypes.IgnoredDefaultNullableProp);
        Assert.NotEqual(default, valueTypes.IgnoredDefaultGuidPropWithInit);
        Assert.NotEqual(default, valueTypes.IgnoredDefaultGuidNullablePropWithInit);
        Assert.NotEqual(default, valueTypes.IgnoredDefaultStringPropWithInit);
        Assert.Equal("Hello World", valueTypes.IgnoredDefaultStringPropWithInit);
        Assert.NotEqual(default, valueTypes.IgnoredDefaultIntPropWithInit);
        Assert.Equal(123, valueTypes.IgnoredDefaultIntPropWithInit);
        Assert.NotEqual(default, valueTypes.IgnoredDefaultEnumPropWithInit);
        Assert.Equal(State.Active, valueTypes.IgnoredDefaultEnumPropWithInit);
        Assert.Equal(default, valueTypes.IgnoredDefaultChild);
        Assert.NotEqual(default, valueTypes.IgnoredDefaultChildWithInit);
        Assert.NotEqual(default, valueTypes.IgnoredDefaultChild1WithInit);
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

public class Entity {
    public int DefaultInteger { get; set; }
    public string DefaultString { get; set; }
    public Guid DefaultGuid { get; set; }
    public State DefaultState { get; set; }
    public ChildEntity DefaultTestObject { get; set; }

    [Rules(CreateAction = RuleAction.Allow)]
    public int Integer { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public string String { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public Guid Guid { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public State State { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public ChildEntity TestObject { get; set; }
    [Rules(CreateAction = RuleAction.Allow)]
    public ChildEntity DesendantsTestObject { get; set; }

    [Rules(CreateAction = RuleAction.Ignore)]
    public int IgnoredProp { get; set; }
    [Rules(CreateAction = RuleAction.Ignore)]
    public ChildEntity IgnoredChild { get; set; }

    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public Guid IgnoredDefaultProp { get; set; }
    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public Guid? IgnoredDefaultNullableProp { get; set; }
    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public Guid IgnoredDefaultGuidPropWithInit { get; set; } = Guid.NewGuid();
    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public Guid IgnoredDefaultGuidNullablePropWithInit { get; set; } = Guid.NewGuid();
    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public string IgnoredDefaultStringPropWithInit { get; set; } = "Hello World";
    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public int IgnoredDefaultIntPropWithInit { get; set; } = 123;
    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public State IgnoredDefaultEnumPropWithInit { get; set; } = State.Active;
    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public ChildEntity IgnoredDefaultChild { get; set; }
    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public ChildEntity IgnoredDefaultChildWithInit { get; set; } = new();
    [Rules(CreateAction = RuleAction.IgnoreDefaults)]
    public ChildEntityNoDefaultContructor IgnoredDefaultChild1WithInit { get; set; } = new("One");


    [JsonIgnore]
    public int JsonIgnoredProp { get; set; }
    [JsonIgnore]
    public ChildEntity JsonIgnoredChild { get; set; }
}


public class BlockedPropertiesEntity {

    [Rules(RuleAction.Block)]
    public string DefaultBlockString { get; set; }
        
    [Rules(CreateAction = RuleAction.Block)]
    public string CreateBlockString { get; set; }
        
    [Rules(CreateAction = RuleAction.Block)]
    public ChildEntity BlockTestObject { get; set; }

}

public class ChildEntity {
    public string PropertyOne { get; set; }
    public string PropertyTwo { get; set; }
    public string PropertyThree { get; set; }
    public ChildEntity TestObject { get; set; }
}

public class ChildEntityNoDefaultContructor {
    public ChildEntityNoDefaultContructor(string prop1)
    {
        PropertyOne = prop1;
    }
    public string PropertyOne { get; set; }
    public string PropertyTwo { get; set; }
    public string PropertyThree { get; set; }
    public ChildEntity TestObject { get; set; }
}

class InvalidReferenceTypes {
    public InvalidTestObject TestObject { get; set; }
}

class InvalidTestObject { }

public enum State {
    Unknowm = 0,
    Active = 1,
}
