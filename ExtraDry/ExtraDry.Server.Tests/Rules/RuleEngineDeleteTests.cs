using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Server.Tests.Rules;

public class RuleEngineDeleteTests
{
    [Fact]
    public async Task DeleteRequiresItem()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await rules.DeleteAsync((object?)null, () => { }, () => Task.CompletedTask));
    }

    [Fact]
    public async Task DeleteSoftDeletesByDefault()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());
        var obj = new SoftDeletable();

        var result = await rules.DeleteAsync(obj, () => { }, () => Task.CompletedTask);

        Assert.False(obj.Active);
        Assert.Equal(DeleteResult.Recycled, result);
    }

    [Fact]
    public async Task DeleteHardDeleteBackup()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());
        var obj = new SoftDeletable();
        var deleted = false;

        var result = await rules.DeleteAsync(new object(), () => deleted = true, () => Task.CompletedTask);

        Assert.True(deleted);
        Assert.Equal(DeleteResult.Expunged, result);
    }

    [Fact]
    public async Task DeleteRecycleRequiresItem()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await rules.DeleteAsync((object?)null!, () => Task.CompletedTask, () => Task.CompletedTask));
    }

    [Fact]
    public async Task DeleteRecycleChangesActive()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());
        var obj = new SoftDeletable();

        var result = await rules.DeleteAsync(obj, () => { }, () => Task.CompletedTask);

        Assert.False(obj.Active);
        Assert.Equal(DeleteResult.Recycled, result);
    }

    [Fact]
    public async Task DeleteHardRequiresPrepareAction()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await rules.DeleteAsync(new object(), null!, () => Task.CompletedTask));
    }

    [Fact]
    public async Task DeleteHardPrepareCommitCycle()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());
        int prepared = 0;
        int committed = 0;

        var result = await rules.DeleteAsync(new object(), () => FakePrepare(ref prepared), () => FakeCommit(ref committed));

        Assert.Equal(1, prepared);
        Assert.Equal(2, committed);
        Assert.Equal(DeleteResult.Expunged, result);
    }

    [Fact]
    public async Task DeleteHardFailHardAndSoft()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());
        var result = await rules.DeleteAsync(new object(), () => { }, () => throw new NotImplementedException());
        Assert.Equal(DeleteResult.NotDeleted, result);
    }

    [Fact]
    public async Task DeleteSoftFallback()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());
        var obj = new SoftDeletable();
        var callCount = 0;

        var result = await rules.DeleteAsync(obj, () => { },
            // exception on hard delete (the second call).
            () => { if(callCount++ > 0) { throw new ArgumentException(); } return Task.CompletedTask; }
        );

        Assert.False(obj.Active);
        Assert.Equal(DeleteResult.Recycled, result);
    }

    [Fact]
    public async Task RecycleDoesntChangeOtherValues()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());
        var obj = new SoftDeletable();
        var original = obj.Unchanged;
        var unruled = obj.UnRuled;

        var result = await rules.DeleteAsync(obj, () => { }, () => Task.CompletedTask);

        Assert.Equal(original, obj.Unchanged);
        Assert.Equal(unruled, obj.UnRuled);
        Assert.Equal(DeleteResult.Recycled, result);
    }

    [Fact]
    public async Task SoftDeleteOnInvalidValueException()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());
        var obj = new BadDeleteValueDeletable();

        async Task lambda() => await rules.DeleteAsync(obj, () => { }, () => Task.CompletedTask);

        await Assert.ThrowsAsync<DryException>(lambda);
    }

    [Fact]
    public async Task NullIsValidDeleteValue()
    {
        var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());
        var obj = new ObjectDeletable();

        await rules.DeleteAsync(obj, () => { }, () => Task.CompletedTask);

        Assert.Null(obj.Status);
    }

    private Task<int> FakePrepare(ref int stepStamp) => Task.FromResult(stepStamp = step++);

    private Task<int> FakeCommit(ref int stepStamp) => Task.FromResult(stepStamp = step++);

    private int step = 1;

    [DeleteRule(DeleteAction.Recycle, nameof(Active), false, true)]
    public class SoftDeletable
    {
        public bool Active { get; set; } = true;

        [Rules]
        public int Unchanged { get; set; } = 2;

        public int UnRuled { get; set; } = 3;
    }

    [SuppressMessage("Usage", "DRY1305:DeleteRule on classes should use nameof for property names.", Justification = "Required for testing.")]
    [DeleteRule(DeleteAction.Recycle, "BadName", false, true)]
    public class BadPropertyDeletable
    {
        public bool Active { get; set; } = true;
    }

    [DeleteRule(DeleteAction.Recycle, nameof(Active), "not-bool")]
    public class BadDeleteValueDeletable
    {
        public bool Active { get; set; } = true;
    }

    [DeleteRule(DeleteAction.Recycle, nameof(Status), null)]
    public class ObjectDeletable
    {
        public object Status { get; set; } = new();
    }
}
