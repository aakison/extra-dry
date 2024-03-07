namespace Sample.Components;

/// <summary>
/// Represents an entity in the component services that is specific to a tenant.
/// </summary>
public interface ITenanted
{
    /// <summary>
    /// The partition key for the tenant.
    /// </summary>
    public string Partition { get; set; }
}
