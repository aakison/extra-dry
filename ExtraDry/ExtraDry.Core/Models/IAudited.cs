namespace ExtraDry.Core;

/// <summary>
/// Represents an audited entity, which has a record of the last user or system to update it.
/// </summary>
public interface IAudited
{

    /// <summary>
    /// The last user, system, or agent to update the object.  This is automatically populated 
    /// on save.
    /// </summary>
    public UserTimestamp Audit { get; set; }

}
