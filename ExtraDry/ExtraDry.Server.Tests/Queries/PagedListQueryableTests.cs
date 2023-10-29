using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Models;

public class PagedListQueryableTests {

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
    [InlineData(0, 5)]
    [InlineData(5, 5)]
    public async Task BasicSkipTakePaging(int skip, int take)
    {
        var query = new PageQuery { Skip = skip, Take = take };
        var expected = Models.ToList().Skip(skip).Take(5);

        var actual = await Models.AsQueryable().QueryWith(query).ToPagedCollectionAsync();

        Assert.Equal(expected, actual.Items);
        Assert.Equal(Models.Count, actual.Total);
        Assert.Equal(take, actual.Count);
        Assert.Equal(skip, actual.Start);
        Assert.Null(actual.Filter);
        Assert.Null(actual.Sort);
        Assert.False(actual.IsFullCollection);
        Assert.NotNull(actual.ContinuationToken);
        var token = ContinuationToken.FromString(actual.ContinuationToken);
        Assert.NotNull(token);
    }

    //[Theory]
    //[InlineData("-name")]
    //[InlineData("-NAME")] // case-insensitive
    //public async Task StringToSortedSortsDescending(string sort)
    //{
    //    // filter to get rid of duplicate names
    //    var query = new SortQuery { Filter = "phonetic", Sort = sort };
    //    var expected = Models.ToList().Where(e => e.Type == ModelType.Phonetic).OrderByDescending(e => e.Name);

    //    var actual = await Models.AsQueryable().QueryWith(query).ToSortedCollectionAsync();

    //    Assert.Equal(expected, actual.Items);
    //}

    //[Theory]
    //[InlineData(null)]
    //[InlineData("")]
    //[InlineData(" ")]
    //public async Task EmptyStringToSortIgnoresSort(string sort)
    //{
    //    var query = new SortQuery { Sort = sort };
    //    var expected = Models.ToList();

    //    var actual = await Models.AsQueryable().QueryWith(query).ToSortedCollectionAsync();

    //    Assert.Equal(expected, actual.Items);
    //}

    [Fact]
    public async Task ToSortedIgnoresPaging()
    {
        var query = new PageQuery { Skip = 5, Take = 5, Sort = nameof(Model.Name), Filter = "phonetic" };
        var expected = Models.ToList().Where(e => e.Type == ModelType.Phonetic).OrderBy(e => e.Name);

        var actual = await Models.AsQueryable().QueryWith(query).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task ToFilteredIgnoresPaging()
    {
        var query = new PageQuery { Skip = 5, Take = 5, Sort = nameof(Model.Name) };
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
