namespace ExtraDry.Core;

public class Summary : IVersionable {

    public Summary()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// The unique ID for the object, which defaults to a new globally unique ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     The date that this object was last updated (in UTC), typically this is the date persisted in the database.
    ///     It is not dynamically updated when this object is edited.
    /// </summary>
    public DateTime VersionDate { get; set; }

    /// <summary>
    ///     The System/user that last updated the object
    /// </summary>
    public string VersionBy { get; set; }

    /// <summary>
    ///     The date and time this object was created (in UTC).
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    ///     The System/user that last created the object
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    ///     Indicates that this object is active or inactive.
    ///     I.E. Soft delete.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The name of the template
    /// this can optionally be use in lieu of { Id } when resolving a summary.
    /// </summary>
    public string SummaryName { get; set; }

    /// <summary>
    /// Tag display string (i.e. Tag.ToString(Title)) that extracts information from the tags to display in title text at the top of summaries.
    /// If this is null or empty, or Tag.ToString returns null or empty that this title is not shown.
    /// E.g. "WO - {WorkOrder.Title}"
    /// </summary>
    public string Title { get; set; }

    public string SubTitle { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public string Functional { get; set; }

    public string RelativeDirectory { get; set; }

    public string FileByteSize { get; set; }
}
