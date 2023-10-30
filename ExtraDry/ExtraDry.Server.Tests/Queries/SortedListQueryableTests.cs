using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Models;

public class SortedListQueryableTests {

    [Fact]
    public void QueryableInterfacePublished()
    {
        var filter = new SortQuery();

        var queryable = Models.AsQueryable().QueryWith(filter);

        Assert.NotNull(queryable.ElementType);
        Assert.NotNull(queryable.Expression);
        Assert.NotNull(queryable.Provider);
        Assert.NotNull(queryable.GetEnumerator());
        Assert.NotNull(((System.Collections.IEnumerable)queryable).GetEnumerator());
    }

    [Theory]
    [InlineData("name")]
    [InlineData("Name")]
    [InlineData("+name")]
    [InlineData("+NAME")] // case-insensitive
    public async Task StringToSortedSortsAscending(string sort)
    {
        // filter to get rid of duplicate names
        var query = new SortQuery { Filter = "phonetic", Sort = sort };
        var expected = Models.ToList().Where(e => e.Type == ModelType.Phonetic).OrderBy(e => e.Name);

        var actual = await Models.AsQueryable().QueryWith(query).ToSortedCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Theory]
    [InlineData("-name")]
    [InlineData("-NAME")] // case-insensitive
    public void StringToSortedSortsDescending(string sort)
    {
        // filter to get rid of duplicate names
        var query = new SortQuery { Filter = "phonetic", Sort = sort };
        var expected = Models.ToList().Where(e => e.Type == ModelType.Phonetic).OrderByDescending(e => e.Name);

        var actual = Models.AsQueryable().QueryWith(query).ToSortedCollection();

        Assert.Equal(expected, actual.Items);
    }

    [Theory]
    [InlineData("-name")]
    [InlineData("-NAME")] // case-insensitive
    public async Task StringToSortedSortsDescendingAsync(string sort)
    {
        // filter to get rid of duplicate names
        var query = new SortQuery { Filter = "phonetic", Sort = sort };
        var expected = Models.ToList().Where(e => e.Type == ModelType.Phonetic).OrderByDescending(e => e.Name);

        var actual = await Models.AsQueryable().QueryWith(query).ToSortedCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task EmptyStringToSortIgnoresSort(string sort)
    {
        var query = new SortQuery { Sort = sort };
        var expected = Models.ToList();

        var actual = await Models.AsQueryable().QueryWith(query).ToSortedCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task StringToFilteredIgnoresSort()
    {
        var query = new SortQuery { Sort = nameof(Model.Name) };
        var expected = Models.ToList();

        var actual = await Models.AsQueryable().QueryWith(query).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    private readonly List<Model> Models = new() {
        new Model { Id = 1, Name = "Alpha", Soundex = "A410", Type = ModelType.Greek, Notes = "Common with phonetic" },
        new Model { Id = 2, Name = "Beta", Soundex = "B300", Type = ModelType.Greek },
        new Model { Id = 3, Name = "Gamma", Soundex = "G500", Type = ModelType.Greek },
        new Model { Id = 4, Name = "Delta", Soundex = "D430", Type = ModelType.Greek, Notes = "Common with phonetic" },
        new Model { Id = 5, Name = "Epsilon", Soundex = "E124", Type = ModelType.Greek },
        new Model { Id = 6, Name = "Zeta", Soundex = "Z300", Type = ModelType.Greek },
        new Model { Id = 7, Name = "Alpha", Soundex = "A410", Type = ModelType.Phonetic, Notes = "Common with Greek" },
        new Model { Id = 8, Name = "Bravo", Soundex = "B610", Type = ModelType.Phonetic },
        new Model { Id = 9, Name = "Charlie", Soundex = "C640", Type = ModelType.Phonetic },
        new Model { Id = 10, Name = "Delta", Soundex = "D430", Type = ModelType.Phonetic, Notes = "Common with Greek" },
        new Model { Id = 11, Name = "Echo", Soundex = "E200", Type = ModelType.Phonetic },
        new Model { Id = 12, Name = "Foxtrot", Soundex = "F236", Type = ModelType.Phonetic },
        new Model { Id = 13, Name = "Foxxy", Soundex = "F200", Type = ModelType.Hendrix, Notes = "Jimi" },
    };

}
