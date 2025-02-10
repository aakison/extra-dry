using Bunit;
using ExtraDry.Blazor.Components.Internal;

namespace ExtraDry.Blazor.Tests.Internals;

public class SelectionSetStaticTests
{

    [Fact]
    public void AccessorHasSelectionSet()
    {
        var key1 = new object();
        
        var accessor = new SelectionSetAccessor(key1);

        Assert.NotNull(accessor.SelectionSet);
    }

    [Fact]
    public void DifferentKeysAreDifferentSelectionSets()
    {
        var key1 = new object();
        var key2 = new object();

        var accessor1 = new SelectionSetAccessor(key1);
        var accessor2 = new SelectionSetAccessor(key2);

        Assert.NotEqual(accessor1.SelectionSet, accessor2.SelectionSet);
    }

    [Fact]
    public void SameKeyAreSameSelectionSet()
    {
        var key = new object();

        var accessor1 = new SelectionSetAccessor(key);
        var accessor2 = new SelectionSetAccessor(key);

        Assert.Equal(accessor1.SelectionSet, accessor2.SelectionSet);
    }
}
