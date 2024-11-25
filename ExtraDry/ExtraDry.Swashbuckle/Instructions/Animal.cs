using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// A sample entity of 'animals', that is used to demonstrate the features of the API.
/// </summary>
public class Animal : IHierarchyEntity<Animal>, IResourceIdentifiers {

    /// <summary>
    /// The DAO primary key.
    /// </summary> 
    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    /// <inheritdoc />
    public Guid Uuid { get; set; }

    /// <summary>
    /// The name of the class of animals, or of a specific animal.
    /// </summary>
    /// <example>Elephant</example>
    [Filter(FilterType.Contains)]
    public required string Title { get; set; }

    /// <summary>
    /// A description of the animal.
    /// </summary>
    /// <example>
    /// A large herbivorous mammal with a trunk, long tusks, and flapping ears, native to Africa 
    /// and Asia.
    /// </example>
    public string Description { get; set; } = string.Empty;

    /// <inheritdoc cref="IResourceIdentifiers.Slug" />
    /// <example>elephant</example>
    public string Slug { get; set; } = string.Empty;

    /// <inheritdoc cref="IHierarchyEntity{T}.Parent" />
    [JsonConverter(typeof(ResourceReferenceConverter<Animal>))]
    public Animal? Parent { get; set; }

    /// <inheritdoc cref="IHierarchyEntity.Lineage" />
    [JsonIgnore]
    public required HierarchyId Lineage { get; set; }

    /// <inheritdoc cref="IHierarchyEntity.Lineage" />
    /// <remarks>
    /// This approach is used as OpenAPI treats HierarchyId as an object and cannot then provide
    /// a string example, or have it declared as read-only.
    /// </remarks>
    /// <example>/1/3/10/</example>
    [JsonPropertyName("lineage")]
    public string Ancestory => Lineage.ToString();
}
