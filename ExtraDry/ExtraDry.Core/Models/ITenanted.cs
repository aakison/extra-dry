namespace ExtraDry.Core;

/// <summary>
/// Represents an entity in the component services that is specific to a tenant.
/// </summary>
public interface ITenanted : IUniqueIdentifier
{
    /// <summary>
    /// The partition key for the tenant.  
    /// </summary>
    public string Tenant { get; set; }

}
