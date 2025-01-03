using ExtraDry.Blazor.Components.Internal;

namespace ExtraDry.Blazor.Tests.Internals;

public class SelectionSetEventTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void EventOnClear(bool multi)
    {
        var set = new SelectionSet() { MultipleSelect = multi };

        set.Add(obj1);
        set.Changed += (s, e) => { sender = s; args = e; };
        set.Clear();

        Assert.NotNull(sender);
        Assert.Equal(set, sender);
        Assert.NotNull(args);
        Assert.Equal(SelectionSetChangedType.Cleared, args?.Type);
        Assert.NotNull(args!.Added);
        Assert.NotNull(args!.Removed);
        Assert.Empty(args!.Added!);
        Assert.Empty(args!.Removed!);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void NoEventOnClearWhenEmpty(bool multi)
    {
        var set = new SelectionSet() { MultipleSelect = multi };

        set.Changed += (s, e) => { sender = s; args = e; };
        set.Clear();

        Assert.Null(sender);
        Assert.Null(args);
    }

    [Fact]
    public void ExclusiveImplementationEventOnClear()
    {
        var set = new SelectionSet() { MultipleSelect = true };

        set.SelectAll();
        set.Remove(obj1);
        set.Changed += (s, e) => { sender = s; args = e; };
        set.Clear();

        Assert.NotNull(sender);
        Assert.Equal(set, sender);
        Assert.NotNull(args);
        Assert.Equal(SelectionSetChangedType.Cleared, args?.Type);
        Assert.NotNull(args!.Added);
        Assert.NotNull(args!.Removed);
        Assert.Empty(args!.Added);
        Assert.Empty(args!.Removed);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void EventOnAdd(bool multi)
    {
        var set = new SelectionSet() { MultipleSelect = multi };

        set.Changed += (s, e) => { sender = s; args = e; };
        set.Add(obj1);

        Assert.NotNull(sender);
        Assert.Equal(set, sender);
        Assert.NotNull(args);
        Assert.Equal(SelectionSetChangedType.Added, args?.Type);
        Assert.NotNull(args!.Added);
        Assert.NotNull(args!.Removed);
        Assert.Single(args!.Added);
        Assert.Equal(obj1, args?.Added.First());
        Assert.Empty(args!.Removed);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void NoEventOnAddDuplicate(bool multi)
    {
        var set = new SelectionSet() { MultipleSelect = multi };

        set.Add(obj1);
        set.Changed += (s, e) => { sender = s; args = e; };
        set.Add(obj1);

        Assert.Null(sender); // event shouldn't fire on duplicate.
        Assert.Null(args);
    }

    [Fact]
    public void SingleSelectEventChangesOnAdd()
    {
        var set = new SelectionSet() { MultipleSelect = false };

        set.Changed += (s, e) => { sender = s; args = e; };
        set.Add(obj1);
        set.Add(obj2);

        Assert.NotNull(sender);
        Assert.Equal(set, sender);
        Assert.NotNull(args);
        Assert.Equal(SelectionSetChangedType.Changed, args?.Type);
        Assert.NotNull(args!.Added);
        Assert.Single(args!.Added);
        Assert.Equal(obj2, args?.Added.First());
        Assert.NotNull(args!.Removed);
        Assert.Single(args!.Removed);
        Assert.Equal(obj1, args?.Removed.First());
    }

    [Fact]
    public void MultiSelectEventOnMultipleAdd()
    {
        var set = new SelectionSet() { MultipleSelect = true };

        set.Changed += (s, e) => { sender = s; args = e; };
        set.Add(obj1);
        set.Add(obj2);

        Assert.NotNull(sender);
        Assert.Equal(set, sender);
        Assert.NotNull(args);
        Assert.Equal(SelectionSetChangedType.Added, args?.Type);
        Assert.NotNull(args!.Added);
        Assert.Single(args!.Added);
        Assert.Equal(obj2, args!.Added.First());
        Assert.NotNull(args!.Removed);
        Assert.Empty(args!.Removed);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void EventOnRemove(bool multi)
    {
        var set = new SelectionSet() { MultipleSelect = multi };

        set.Add(obj1);
        set.Changed += (s, e) => { sender = s; args = e; };
        set.Remove(obj1);

        Assert.NotNull(sender);
        Assert.Equal(set, sender);
        Assert.NotNull(args);
        Assert.Equal(SelectionSetChangedType.Removed, args?.Type);
        Assert.NotNull(args!.Added);
        Assert.NotNull(args!.Removed);
        Assert.Empty(args!.Added);
        Assert.Single(args!.Removed);
        Assert.Equal(obj1, args?.Removed.First());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void NoEventOnRemoveMissing(bool multi)
    {
        var set = new SelectionSet() { MultipleSelect = multi };

        set.Changed += (s, e) => { sender = s; args = e; };
        set.Remove(obj1);

        Assert.Null(sender); // event shouldn't fire on duplicate.
        Assert.Null(args);
    }

    [Fact]
    public void EventOnSelectAll()
    {
        var set = new SelectionSet() { MultipleSelect = true };

        set.Changed += (s, e) => { sender = s; args = e; };
        set.SelectAll();

        Assert.NotNull(sender);
        Assert.Equal(set, sender);
        Assert.NotNull(args);
        Assert.Equal(SelectionSetChangedType.SelectAll, args?.Type);
        Assert.NotNull(args!.Added);
        Assert.NotNull(args!.Removed);
        Assert.Empty(args!.Added);
        Assert.Empty(args!.Removed);
    }

    [Fact]
    public void NoEventOnMultipleSelectAll()
    {
        var set = new SelectionSet() { MultipleSelect = true };

        set.SelectAll();
        set.Changed += (s, e) => { sender = s; args = e; };
        set.SelectAll();

        Assert.Null(sender); // second select all doesn't change selection.
        Assert.Null(args);
    }

    private object? sender;

    private SelectionSetChangedEventArgs? args;

    private readonly object obj1 = new();

    private readonly object obj2 = new();
}
