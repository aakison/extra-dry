using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Server.Tests.Rules;

public class RuleEngineDeleteTests {

    [Fact]
    public void DeleteRequiresItem()
    {
        var rules = new RuleEngine(new ServiceProviderStub());

        Assert.Throws<ArgumentNullException>(() => rules.Delete((object?)null, NoOp, NoOp));
    }

    [Fact]
    public void DeleteSoftDeletesByDefault()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new SoftDeletable();

        var result = rules.TrySoftDelete(obj);

        Assert.False(obj.Active);
        Assert.Equal(DeleteResult.SoftDeleted, result);
    }

    [Fact]
    public void DeleteHardDeleteBackup()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new SoftDeletable();
        var deleted = false;

        var result = rules.Delete(new object(), () => deleted = true, NoOp);

        Assert.True(deleted);
        Assert.Equal(DeleteResult.HardDeleted, result);
    }

    [Fact]
    public void DeleteSoftRequiresItem()
    {
        var rules = new RuleEngine(new ServiceProviderStub());

        Assert.Throws<ArgumentNullException>(() => rules.TrySoftDelete((object?)null!));
    }

    [Fact]
    public void DeleteSoftChangesActive()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new SoftDeletable();

        var result = rules.TrySoftDelete(obj);

        Assert.False(obj.Active);
        Assert.Equal(DeleteResult.SoftDeleted, result);
    }

    [Fact]
    public void DeleteHardRequiresItem()
    {
        var rules = new RuleEngine(new ServiceProviderStub());

        Assert.ThrowsAsync<ArgumentNullException>(() => rules.TryHardDeleteAsync((object?)null!));
    }

    [Fact]
    public void DeleteHardRequiresPrepareAction()
    {
        var rules = new RuleEngine(new ServiceProviderStub());

        Assert.ThrowsAsync<ArgumentNullException>(() => rules.TryHardDeleteAsync(new object(), null!, NoOp));
    }

    [Fact]
    public void DeleteHardRequiresCommitAction()
    {
        var rules = new RuleEngine(new ServiceProviderStub());

        Assert.ThrowsAsync<ArgumentNullException>(
            () => rules.TryHardDeleteAsync(new object(), NoOp, null!)
        );
    }

    [Fact]
    public async Task DeleteHardPrepareCommitCycle()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        int prepared = 0;
        int committed = 0;

        var result = await rules.TryHardDeleteAsync(new object(), () => FakePrepare(ref prepared), () => FakeCommit(ref committed));

        Assert.Equal(1, prepared);
        Assert.Equal(2, committed);
        Assert.Equal(DeleteResult.HardDeleted, result);
    }

    [Fact]
    public async Task DeleteHardFailHardAndSoft()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var result = await rules.TryHardDeleteAsync(new object(), NoOp, () => throw new NotImplementedException());
        Assert.Equal(DeleteResult.NotDeleted, result);
    }

    [Fact]
    public void DeleteSoftFallback()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new SoftDeletable();
        var callCount = 0;

        var result = rules.Delete(obj, NoOp,
            () => { if(callCount++ > 0) { throw new Exception(); } } // exception on hard delete (the second call).
        );

        Assert.False(obj.Active);
        Assert.Equal(DeleteResult.SoftDeleted, result);
    }

    [Fact]
    public void SoftDeleteDoesntChangeOtherValues()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new SoftDeletable();
        var original = obj.Unchanged;
        var unruled = obj.UnRuled;

        var result = rules.TrySoftDelete(obj);

        Assert.Equal(original, obj.Unchanged);
        Assert.Equal(unruled, obj.UnRuled);
        Assert.Equal(DeleteResult.SoftDeleted, result);
    }

    [Fact]
    public void SoftDeleteOnInvalidProperty()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new BadPropertyDeletable();

        var result = rules.TrySoftDelete(obj);

        Assert.Equal(DeleteResult.NotDeleted, result);
    }

    [Fact]
    public void SoftDeleteOnInvalidValueException()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new BadDeleteValueDeletable();

        var lambda = () => {
            _ = rules.TrySoftDelete(obj);
        };

        Assert.Throws<DryException>(lambda);
    }

    [Fact]
    public void NullIsValidDeleteValue()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var obj = new ObjectDeletable();

        rules.Delete(obj, NoOp, NoOp);

        Assert.Null(obj.Status);
    }

    private static void NoOp() { }

    private Task FakePrepare(ref int stepStamp) => Task.FromResult(stepStamp = step++);

    private Task FakeCommit(ref int stepStamp) => Task.FromResult(stepStamp = step++);

    private int step = 1;

    [SoftDeleteRule(nameof(Active), false, true)]
    public class SoftDeletable {
        public bool Active { get; set; } = true;

        [Rules]
        public int Unchanged { get; set; } = 2;

        public int UnRuled { get; set; } = 3;
    }

    [SuppressMessage("Usage", "DRY1305:SoftDelete on classes should use nameof for property names.", Justification = "Required for testing.")]
    [SoftDeleteRule("BadName", false, true)]
    public class BadPropertyDeletable
    {
        public bool Active { get; set; } = true;
    }

    [SoftDeleteRule(nameof(Active), "not-bool")]
    public class BadDeleteValueDeletable
    {
        public bool Active { get; set; } = true;
    }

    [SoftDeleteRule(nameof(Status), null)]
    public class ObjectDeletable
    {
        public object Status { get; set; } = new();
    }

}
