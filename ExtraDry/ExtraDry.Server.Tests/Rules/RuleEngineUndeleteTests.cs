namespace ExtraDry.Server.Tests.Rules;

public class RuleEngineUndeleteTests {

    [Fact]
    public void DeleteRequiresItem()
    {
        var rules = new RuleEngine(new ServiceProviderStub());

        var lambda = () => {
            rules.Undelete((object?)null);
        };

        Assert.Throws<ArgumentNullException>(lambda);
    }

    [Fact]
    public void UndeleteWorksOnEnum()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new SoftDeletable();
        rules.Delete(obj);

        var result = rules.Undelete(obj);

        Assert.Equal(UndeleteResult.Undeleted, result);
    }

    [Fact]
    public void NotDeletableDoesNothing()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new NotDeletable();

        var result = rules.Undelete(obj);

        Assert.Equal(UndeleteResult.NotUndeleted, result);
    }

    [Fact]
    public void NotUndeletableDoesNothing()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new NotUndeletable();
        rules.Delete(obj);

        var result = rules.Undelete(obj);

        Assert.Equal(UndeleteResult.NotUndeleted, result);
    }

    [Fact]
    public void BadPropertyException()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new BadProperty();

        var lambda = () => {
            rules.Undelete(obj);
        };

        Assert.Throws<DryException>(lambda);
    }

    [Fact]
    public void NotProperlyDeletedDontUndelete()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new SoftDeletable();

        var result = rules.Undelete(obj);

        Assert.Equal(UndeleteResult.NotUndeleted, result);
    }

    [Fact]
    public void BadUndeleteValueException()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new BadUndeleteValue();
        rules.Delete(obj);

        var lambda = () => {
            rules.Undelete(obj);
        };

        Assert.Throws<DryException>(lambda);
    }

    [SoftDeleteRule(nameof(State), State.Deleted, State.Active)]
    public class SoftDeletable {
        public State State { get; set; } = State.Inactive;
    }

    public class NotDeletable {
        public State State { get; set; } = State.Inactive;
    }

    [SoftDeleteRule(nameof(State), State.Deleted)]
    public class NotUndeletable {
        public State State { get; set; } = State.Inactive;
    }

    [SoftDeleteRule("BadName", State.Deleted, State.Active)]
    public class BadProperty {
        public State State { get; set; } = State.Inactive;
    }

    [SoftDeleteRule(nameof(State), State.Deleted, "BadValue")]
    public class BadUndeleteValue {
        public State State { get; set; } = State.Inactive;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum State
    {
        Active,
        Inactive,
        Deleted,
    }
}
