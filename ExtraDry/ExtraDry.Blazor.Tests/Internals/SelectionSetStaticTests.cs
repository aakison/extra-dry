using ExtraDry.Blazor.Components.Internal;
using Xunit;

namespace ExtraDry.Blazor.Tests.Internals;

public class SelectionSetStaticTests {

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void NullOnMissingDataLookup(string key)
    {
        var set = SelectionSet.Lookup(key);

        Assert.Null(set);
    }

    [Fact]
    public void ReturnRegistered()
    {
        var key1 = new object();
        var key2 = new object();

        SelectionSet.Register(key1);
        SelectionSet.Register(key2);
        var set1 = SelectionSet.Lookup(key1);
        var set2 = SelectionSet.Lookup(key2);
        var set3 = SelectionSet.Lookup(key1);

        Assert.NotNull(set1);
        Assert.NotNull(set2);
        Assert.NotEqual(set1, set2);
        Assert.Equal(set1, set3);
    }

    [Fact]
    public void NullAfterDeregister()
    {
        var key1 = new object();
        var key2 = new object();
        SelectionSet.Register(key1);
        SelectionSet.Register(key2);

        SelectionSet.Deregister(key1);
        var set1 = SelectionSet.Lookup(key1);

        Assert.Null(set1);
    }

}
