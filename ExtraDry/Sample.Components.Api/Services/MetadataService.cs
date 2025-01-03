namespace Sample.Components.Api.Services;

/// <summary>
/// Standard services for Metadata, each requiring a tenant partition for scope.
/// </summary>
//public class MetadataService(
//    ComponentContext database,
//    RuleEngine rules)
//{
//    public async Task<Metadata> CreateMetadataAsync(Component component, Metadata exemplar)
//    {
//        var metadata = await rules.CreateAsync(exemplar);
//        metadata.Partition = component.Partition;
//        database.Metadata.Add(metadata);
//        await database.SaveChangesAsync();
//        return metadata;
//    }

// public async Task<Metadata?> TryRetrieveMetadataAsync(Component component) { var metadata =
// await database.Metadata.WithPartitionKey(component.Partition).FirstOrDefaultAsync(e => e.Uuid ==
// component.Uuid); return metadata; }

// public async Task<Metadata> RetrieveMetadataAsync(Component component) { return await
// TryRetrieveMetadataAsync(component) ?? throw new ArgumentException($"Metadata for Component
// '{component.Uuid}' not found", nameof(component)); }

// public async Task<Metadata> UpdateMetadataAsync(Component component, Metadata exemplar) { var
// existing = await RetrieveMetadataAsync(component); await rules.UpdateAsync(exemplar, existing);
// await database.SaveChangesAsync(); return existing; }

// public async Task DeleteMetadataAsync(Component component) { var existing = await
// RetrieveMetadataAsync(component); await rules.DeleteAsync(existing,
// database.Metadata.Remove(existing), database.SaveChangesAsync()); }

//}
