namespace ExtraDry.Core;

/// <summary>
/// Represents a revisioned entity, which has and retains the last user to update it.
/// </summary>
public interface IRevisioned
{

    /// <summary>
    /// The last human user to update the object, this may not be the most recent update which may 
    /// have been made by the system or an automated agent.  This is automatically populated 
    /// on save.
    /// </summary>
    public UserTimestamp Revision { get; set; }

}
