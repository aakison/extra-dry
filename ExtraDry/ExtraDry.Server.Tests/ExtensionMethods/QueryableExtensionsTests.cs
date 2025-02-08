namespace ExtraDry.Core.Server.Tests.ExtensionMethods;

public class QueryableExtensionsTests
{
    [Fact]
    public void NoSortSpecifiedUseIdStabilizer()
    {
        var list = new List<IdConventionKey> {
            new() { Id = 1 },
            new() { Id = 3 },
            new() { Id = 2 },
        };
        var query = new SortQuery() { Stabilization = SortStabilization.PrimaryKey };

        var sorted = list.AsQueryable()
            .Sort(query)
            .ToList();

        Assert.Equal(3, sorted.Count);
        Assert.Equal(1, sorted.First().Id);
        Assert.Equal(3, sorted.Last().Id);
    }

    [Fact]
    public void NoSortSpecifiedUseClassNameStabilizer()
    {
        var list = new List<ClassNameConventionKey> {
            new() { ClassNameConventionKeyId = 1 },
            new() { ClassNameConventionKeyId = 3 },
            new() { ClassNameConventionKeyId = 2 },
        };
        var query = new SortQuery() { Stabilization = SortStabilization.PrimaryKey };

        var sorted = list.AsQueryable().Sort(query).ToList();

        Assert.Equal(3, sorted.Count);
        Assert.Equal(1, sorted.First().ClassNameConventionKeyId);
        Assert.Equal(3, sorted.Last().ClassNameConventionKeyId);
    }

    [Fact]
    public void NoSortSpecifiedUseKeyAttributeStabilizer()
    {
        var list = new List<KeyAttributeEntity> {
            new() { PrimaryKey = 1 },
            new() { PrimaryKey = 3 },
            new() { PrimaryKey = 2 },
        };
        var query = new SortQuery() { Stabilization = SortStabilization.PrimaryKey };

        var sorted = list.AsQueryable().Sort(query).ToList();

        Assert.Equal(3, sorted.Count);
        Assert.Equal(1, sorted.First().PrimaryKey);
        Assert.Equal(3, sorted.Last().PrimaryKey);
    }

    [Fact]
    public void NoSortSpecifiedNoStabilizerException()
    {
        var list = new List<NoImplicitStabilizer> {
            new() { PrimaryKey = 1 },
            new() { PrimaryKey = 3 },
            new() { PrimaryKey = 2 },
        };
        var query = new SortQuery();

        Assert.Throws<DryException>(() => list.AsQueryable().Sort(query));
    }

    [Fact]
    public void NoSortSpecifiedCompositeKeyStabilizerException()
    {
        var list = new List<CompositeKeyStabilizer> {
            new() { FirstPartKey = 1 },
            new() { FirstPartKey = 3 },
            new() { FirstPartKey = 2 },
        };
        var query = new SortQuery();

        Assert.Throws<DryException>(() => list.AsQueryable().Sort(query));
    }

    public class IdConventionKey
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Payload { get; set; } = string.Empty;
    }

    public class ClassNameConventionKey
    {
        [JsonIgnore]
        public int ClassNameConventionKeyId { get; set; }

        public string Payload { get; set; } = string.Empty;
    }

    public class KeyAttributeEntity
    {
        [Key]
        [JsonIgnore]
        public int PrimaryKey { get; set; }

        public string Payload { get; set; } = string.Empty;
    }

    public class NoImplicitStabilizer
    {
        public int PrimaryKey { get; set; }

        public string Payload { get; set; } = string.Empty;
    }

    public class CompositeKeyStabilizer
    {
        [Key]
        [JsonIgnore]
        public int FirstPartKey { get; set; }

        [Key]
        [JsonIgnore]
        public int SecondPartKey { get; set; }

        public string Payload { get; set; } = string.Empty;
    }
}
