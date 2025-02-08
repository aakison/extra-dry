using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Models;

public class BaseQueryableTests
{
    [Fact]
    public void QueryableInterfacePublished()
    {
        var query = new BaseQueryable<Model>(Models.AsQueryable());

        Assert.NotNull(query.ElementType);
        Assert.NotNull(query.Expression);
        Assert.NotNull(query.Provider);
        Assert.NotNull(query.GetEnumerator());
        Assert.NotNull(((System.Collections.IEnumerable)query).GetEnumerator());
    }

    [Fact]
    public void QueryReturnsEverything()
    {
        var query = new BaseQueryable<Model>(Models.AsQueryable());
        var expected = Models.ToList();

        var actual = query.ToBaseCollection();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task QueryReturnsEverythingAsync()
    {
        var query = new BaseQueryable<Model>(Models.AsQueryable());
        var expected = Models.ToList();

        var actual = await query.ToBaseCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task QueryReturnsAsyncEnumerable()
    {
        var models = new ModelsBaseCollection { Models = Models };
        var expected = Models.OrderBy(e => e.Id).ToList();

        var actual = await models.ToBaseCollectionAsync();

        Assert.Equal(expected, actual.Items.OrderBy(e => e.Id)); // async collection is not sorted
    }

    // Support the IAsyncEnumerable interface, enable testing of the async methods within the
    // BaseQueryable which is used by DbSet implementations.
    public class ModelsBaseCollection : BaseQueryable<Model>, IAsyncEnumerable<Model>
    {
        public IAsyncEnumerator<Model> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new ModelsEnumerator { Models = Models };
        }

        public class ModelsEnumerator : IAsyncEnumerator<Model>
        {
            public Model Current => Models[index];

            public ValueTask DisposeAsync()
            {
                GC.SuppressFinalize(this);
                return ValueTask.CompletedTask;
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return ValueTask.FromResult(++index < Models.Count);
            }

            private int index = -1;

            public required List<Model> Models { get; init; }
        }

        public required List<Model> Models { get; init; }
    }

    private readonly List<Model> Models = [
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
    ];
}
