using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Server.Tests.Rules;

public class RuleEngineUndeleteTests {

    [Fact]
    public async Task DeleteRequiresItem()
    {
        var rules = new RuleEngine(new ServiceProviderStub());

        async Task lambda() => await rules.RestoreAsync((object?)null);

        await Assert.ThrowsAsync<ArgumentNullException>(lambda);
    }

    [Fact]
    public async Task UndeleteWorksOnEnum()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new SoftDeletable();
        await rules.DeleteAsync(obj, () => { }, () => Task.CompletedTask);

        var result = await rules.RestoreAsync(obj);

        Assert.Equal(RestoreResult.Restored, result);
    }

    [Fact]
    public async Task NotDeletableDoesNothing()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new NotDeletable();

        var result = await rules.RestoreAsync(obj);

        Assert.Equal(RestoreResult.NotRestored, result);
    }

    [Fact]
    public async Task NotUndeletableDoesNothing()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new NotUndeletable();
        await rules.DeleteAsync(obj, () => { }, () => Task.CompletedTask);

        var result = await rules.RestoreAsync(obj);

        Assert.Equal(RestoreResult.NotRestored, result);
    }

    [Fact]
    public async Task BadPropertyException()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new BadProperty();

        async Task lambda() => await rules.RestoreAsync(obj);

        await Assert.ThrowsAsync<DryException>(lambda);
    }

    [Fact]
    public async Task NotProperlyDeletedDontUndelete()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new SoftDeletable();

        var result = await rules.RestoreAsync(obj);

        Assert.Equal(RestoreResult.NotRestored, result);
    }

    [Fact]
    public async Task BadUndeleteValueException()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new BadUndeleteValue();
        await rules.DeleteAsync(obj, () => { }, () => Task.CompletedTask);

        async Task lambda() => await rules.RestoreAsync(obj);

        await Assert.ThrowsAsync<DryException>(lambda);
    }

    [Fact]
    public async Task NullIsValidUndeleteValue()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new NullUndelete();
        await rules.DeleteAsync(obj, () => { }, () => Task.CompletedTask);

        await rules.RestoreAsync(obj);

        Assert.Null(obj.Status);
    }

    [DeleteRule(DeleteAction.Recycle, nameof(Status), "Deleted", null)]
    public class NullUndelete {
        public string Status { get; set; } = "Active";
    }

    [DeleteRule(DeleteAction.Recycle, nameof(State), State.Deleted, State.Active)]
    public class SoftDeletable {
        public State State { get; set; } = State.Inactive;
    }

    public class NotDeletable {
        public State State { get; set; } = State.Inactive;
    }

    [DeleteRule(DeleteAction.Recycle, nameof(State), State.Deleted)]
    public class NotUndeletable {
        public State State { get; set; } = State.Inactive;
    }

    [SuppressMessage("Usage", "DRY1305:DeleteRule on classes should use nameof for property names.", Justification = "Required for testing.")]
    [DeleteRule(DeleteAction.Recycle, "BadName", State.Deleted, State.Active)]
    public class BadProperty {
        public State State { get; set; } = State.Inactive;
    }

    [DeleteRule(DeleteAction.Recycle, nameof(State), State.Deleted, "BadValue")]
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
