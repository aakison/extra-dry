using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Core;

/// <summary>
/// Represents an event in time that was initiated by a user, system, or agent.
/// </summary>
[Owned]
public class UserTimestamp
{
    /// <summary>
    /// The user that created or updated the object. This is designed to be automatically updated
    /// by a database context aspect when the object is updated in the database. This should not be
    /// populated with user personally identifiable information (PII), consider a User UUID or
    /// similar value for this field.
    /// </summary>
    [StringLength(StringLength.Line)]
    public string User { get; set; } = "";

    /// <summary>
    /// The UTC date/time the object was created or updated. This is designed to be automatically
    /// updated by a database context aspect when the object is updated in the database.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.MinValue;
}
