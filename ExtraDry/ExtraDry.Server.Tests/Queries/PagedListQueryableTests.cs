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
    public void BasicSkipTakePaging(int skip, int take)
    {
        var query = new PageQuery { Skip = skip, Take = take };
        var expected = Models.ToList().Skip(skip).Take(5);

        var actual = Models.AsQueryable().QueryWith(query).ToPagedCollection();

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
        Assert.Equal(skip + take, token.Skip);
        Assert.Equal(take, token.Take);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(5, 5)]
    public void NextResultsUsingOnlyToken(int skip, int take)
    {
        var query = new PageQuery { Token = new ContinuationToken(null, null, skip, take).ToString() };
        var expected = Models.ToList().Skip(skip).Take(take);

        var actual = Models.AsQueryable().QueryWith(query).ToPagedCollection();

        Assert.Equal(expected.Count(), actual.Count);
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
        Assert.Equal(skip + take, token.Skip);
        Assert.Equal(take, token.Take);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(5, 5)]
    public async Task BasicSkipTakePagingAsync(int skip, int take)
    {
        var query = new PageQuery { Skip = skip, Take = take };
        var expected = Models.ToList().Skip(skip).Take(take);

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
        Assert.Equal(skip + take, token.Skip);
        Assert.Equal(take, token.Take);
    }

    [Fact]
    public async Task SkipTakeLastPageAsync()
    {
        var query = new PageQuery { Skip = 0, Take = 15 };
        var expected = Models.ToList().Skip(0).Take(15);

        var actual = await Models.AsQueryable().QueryWith(query).ToPagedCollectionAsync();

        Assert.Equal(expected, actual.Items);
        Assert.Equal(Models.Count, actual.Total);
        Assert.Equal(13, actual.Count);
        Assert.Equal(0, actual.Start);
        Assert.Null(actual.Filter);
        Assert.Null(actual.Sort);
        Assert.True(actual.IsFullCollection);
        Assert.NotNull(actual.ContinuationToken);
        var token = ContinuationToken.FromString(actual.ContinuationToken);
        Assert.NotNull(token);
        Assert.Equal(0, token.Skip);
        Assert.Equal(15, token.Take);
    }

    [Fact]
    public async Task SkipTakePastLastPageAsync()
    {
        var query = new PageQuery { Skip = 15, Take = 5 };
        var expected = Models.ToList().Skip(15).Take(5);

        var actual = await Models.AsQueryable().QueryWith(query).ToPagedCollectionAsync();

        Assert.Equal(expected, actual.Items);
        Assert.Equal(Models.Count, actual.Total);
        Assert.Equal(0, actual.Count);
        Assert.Equal(15, actual.Start);
        Assert.Null(actual.Filter);
        Assert.Null(actual.Sort);
        Assert.False(actual.IsFullCollection);
        Assert.NotNull(actual.ContinuationToken);
        var token = ContinuationToken.FromString(actual.ContinuationToken);
        Assert.NotNull(token);
        Assert.Equal(15, token.Skip);
        Assert.Equal(5, token.Take);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(5, 5)]
    public async Task NextResultsUsingOnlyTokenAsync(int skip, int take)
    {
        var query = new PageQuery { Token = new ContinuationToken(null, null, skip, take).ToString() };
        var expected = Models.ToList().Skip(skip).Take(take);

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
        Assert.Equal(skip + take, token.Skip);
        Assert.Equal(take, token.Take);
    }

    [Fact]
    public async Task NextResultsUsingOnlyTokenLastPageAsync()
    {
        var query = new PageQuery { Token = new ContinuationToken(null, null, 10, 5).ToString() };
        var expected = Models.ToList().Skip(10).Take(5);

        var actual = await Models.AsQueryable().QueryWith(query).ToPagedCollectionAsync();

        Assert.Equal(expected, actual.Items);
        Assert.Equal(Models.Count, actual.Total);
        Assert.Equal(3, actual.Count);
        Assert.Equal(10, actual.Start);
        Assert.Null(actual.Filter);
        Assert.Null(actual.Sort);
        Assert.False(actual.IsFullCollection);
        var token = ContinuationToken.FromString(actual.ContinuationToken);
        Assert.NotNull(token);
        Assert.Equal(10, token.Skip);
        Assert.Equal(5, token.Take);
    }

    [Fact]
    public async Task NextResultsUsingOnlyTokenPastLastPageAsync()
    {
        var query = new PageQuery { Token = new ContinuationToken(null, null, 15, 5).ToString() };
        var expected = Models.ToList().Skip(15).Take(5);

        var actual = await Models.AsQueryable().QueryWith(query).ToPagedCollectionAsync();

        Assert.Equal(expected, actual.Items);
        Assert.Equal(Models.Count, actual.Total);
        Assert.Equal(0, actual.Count);
        Assert.Equal(15, actual.Start);
        Assert.Null(actual.Filter);
        Assert.Null(actual.Sort);
        Assert.False(actual.IsFullCollection);
        var token = ContinuationToken.FromString(actual.ContinuationToken);
        Assert.NotNull(token);
        Assert.Equal(15, token.Skip);
        Assert.Equal(5, token.Take);
    }

    [Theory]
    [InlineData(2, null, 0, 5)]
    [InlineData(null, 2, 5, 5)]
    [InlineData(2, 2, 5, 5)]
    public async Task NextResultsUsingTokenAndQueryOverrideAsync(int? querySkip, int? queryTake, int tokenSkip, int tokenTake)
    {
        var query = new PageQuery { Skip = querySkip ?? 0, Take = queryTake ?? 0, Token = new ContinuationToken(null, null, tokenSkip, tokenTake).ToString() };
        var expected = Models.ToList().Skip(querySkip ?? tokenSkip).Take(queryTake ?? tokenTake);

        var actual = await Models.AsQueryable().QueryWith(query).ToPagedCollectionAsync();

        Assert.Equal(expected, actual.Items);
        Assert.Equal(Models.Count, actual.Total);
        Assert.Equal(queryTake ?? tokenTake, actual.Count);
        Assert.Equal(querySkip ?? tokenSkip, actual.Start);
        Assert.Null(actual.Filter);
        Assert.Null(actual.Sort);
        Assert.False(actual.IsFullCollection);
        Assert.NotNull(actual.ContinuationToken);
        var token = ContinuationToken.FromString(actual.ContinuationToken);
        Assert.NotNull(token);
        Assert.Equal((querySkip ?? tokenSkip) + (queryTake ?? tokenTake), token.Skip);
        Assert.Equal(queryTake ?? tokenTake, token.Take);
    }

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
