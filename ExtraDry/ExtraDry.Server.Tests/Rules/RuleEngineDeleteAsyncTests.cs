namespace ExtraDry.Server.Tests.Rules;

public class RuleEngineDeleteAsyncTests {

    [Fact]
    public async Task EntityFrameworkStyleDeleteSoftExecutesSoft()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var item = new SoftDeletable();
        var items = new List<SoftDeletable> { item };

        var result = await rules.DeleteAsync(item, () => items.Remove(item), () => Task.CompletedTask);

        Assert.NotEmpty(items);
        Assert.False(item.Active);
        Assert.Equal(DeleteResult.Recycled, result);
    }

    [Fact]
    public async Task EntityFrameworkStyleDeleteExecutesHard()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        
        var item = new object();
        var items = new List<object> { item };

        var result = await rules.DeleteAsync(item, () => items.Remove(item), SaveChangesAsync);

        Assert.Empty(items);
        Assert.Equal(DeleteResult.Expunged, result);
    }

    [Fact]
    public async Task EntityFrameworkStyleHardDelete()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var item = new object();
        var items = new List<object> { item };

        var result = await rules.ExpungeAsync(item, () => items.Remove(item), SaveChangesAsync);

        Assert.Equal(SaveState.Done, state);
        Assert.Empty(items);
        Assert.Equal(DeleteResult.Expunged, result);
    }

    [Fact]
    public async Task EntityFrameworkStyleHardDeleteAsyncPrepare()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var item = new object();
        var items = new List<object> { item };

        var result = await rules.ExpungeAsync(item, 
            async () => {
                await Task.Delay(1);
                items.Remove(item);
            }, 
            SaveChangesAsync
        );

        Assert.Equal(SaveState.Done, state);
        Assert.Empty(items);
        Assert.Equal(DeleteResult.Expunged, result);
    }

    [Fact]
    public async Task ExpungeShouldNotHavePropertyValues()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var item = new IncorrectRecyclable();
        var items = new List<object> { item };

        Task<DeleteResult> lambda() => rules.DeleteAsync(item, () => items.Remove(item), SaveChangesAsync);

        await Assert.ThrowsAsync<DryException>(lambda);
    }

    private async Task SaveChangesAsync()
    {
        state = SaveState.Processing;
        await Task.Delay(1);
        state = SaveState.Done;
    }

    private SaveState state = SaveState.Pending;

    private enum SaveState {
        Pending = 0,
        Processing = 1,
        Done = 2,
    }

    [DeleteRule(DeleteAction.Recycle, nameof(Active), false, true)]
    public class SoftDeletable {
        public bool Active { get; set; } = true;
    }

    [DeleteRule(DeleteAction.Recycle)]
    public class IncorrectRecyclable
    {
        public bool Active { get; set; } = true;
    }

}
