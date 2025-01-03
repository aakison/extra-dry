using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Internals;

public class ModelDescriptionTests
{
    [Fact]
    public void StabilizerPropertyIsKeyAttribute()
    {
        var modelDescription = new ModelDescription(typeof(KeyAttributeEntity));

        Assert.NotNull(modelDescription);
        Assert.NotNull(modelDescription.StabilizerProperty);
        Assert.Equal("PrimaryKey", modelDescription.StabilizerProperty?.ExternalName);
    }

    [Fact]
    public void StabilizerPropertyIsIdConvention()
    {
        var modelDescription = new ModelDescription(typeof(IdConventionEntity));

        Assert.NotNull(modelDescription);
        Assert.NotNull(modelDescription.StabilizerProperty);
        Assert.Equal("Id", modelDescription.StabilizerProperty?.ExternalName);
    }

    [Fact]
    public void StabilizerPropertyIsClassNameConvention()
    {
        var modelDescription = new ModelDescription(typeof(ClassNameConventionEntity));

        Assert.NotNull(modelDescription);
        Assert.NotNull(modelDescription.StabilizerProperty);
        Assert.Equal("ClassNameConventionEntityId", modelDescription.StabilizerProperty?.ExternalName);
    }

    [Fact]
    public void StabilizerPropertyIsMissing()
    {
        var modelDescription = new ModelDescription(typeof(NoImplicitStabilizer));

        var stabilizerProperty = modelDescription.StabilizerProperty;

        Assert.Null(stabilizerProperty);
    }

    [Fact]
    public void StabilizerPropertyIsCompositeKey()
    {
        var modelDescription = new ModelDescription(typeof(CompositeKeyAttributeEntity));

        var stabilizerProperty = modelDescription.StabilizerProperty;

        Assert.Null(stabilizerProperty);
    }

    [Fact]
    public void SortPropertiesFiltered()
    {
        var modelDescription = new ModelDescription(typeof(SortPropertiesEntity));

        Assert.Equal(2, modelDescription.SortProperties.Count);
        Assert.Equal("Name", modelDescription.SortProperties[0].ExternalName);
        Assert.Equal("ExternalName", modelDescription.SortProperties[1].ExternalName);
    }

    [Fact]
    public void FilterPropertiesFiltered()
    {
        var modelDescription = new ModelDescription(typeof(SortPropertiesEntity));

        Assert.Equal(2, modelDescription.FilterProperties.Count);
        Assert.Equal("Name", modelDescription.FilterProperties[0].ExternalName);
        Assert.Equal("ExternalName", modelDescription.FilterProperties[1].ExternalName);
    }

    public class KeyAttributeEntity
    {
        [Key]
        [JsonIgnore]
        public int PrimaryKey { get; set; }

        public string Payload { get; set; } = string.Empty;
    }

    public class IdConventionEntity
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Payload { get; set; } = string.Empty;
    }

    public class ClassNameConventionEntity
    {
        [JsonIgnore]
        public int ClassNameConventionEntityId { get; set; }

        public string Payload { get; set; } = string.Empty;
    }

    public class NoImplicitStabilizer
    {
        public int PrimaryKey { get; set; }

        public string Payload { get; set; } = string.Empty;
    }

    public class CompositeKeyAttributeEntity
    {
        [Key]
        [JsonIgnore]
        public int PrimaryKey { get; set; }

        [Key]
        [JsonIgnore]
        public int SecondaryKey { get; set; }

        public string Payload { get; set; } = string.Empty;
    }

    public class SortPropertiesEntity
    {
        [Key]
        [JsonIgnore]
        public int Key { get; set; }

        [Filter(FilterType.Contains)]
        public string Name { get; set; } = string.Empty;

        [Filter(FilterType.Equals)]
        [JsonPropertyName("externalName")]
        public string InternalName { get; set; } = string.Empty;

        public object Entity { get; set; } = new();

        public ICollection<object> Collection { get; set; } = [];

        [JsonIgnore]
        public string Ignored { get; set; } = string.Empty;

        [NotMapped]
        public string NotMapped { get; set; } = string.Empty;
    }
}
