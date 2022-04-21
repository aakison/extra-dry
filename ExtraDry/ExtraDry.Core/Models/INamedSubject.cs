namespace ExtraDry.Core;

/// <summary>
/// Represents the interface that defines the names of Subjects.
/// </summary>
public interface INamedSubject {

    /// <summary>
    /// A Code that is used to uniquely identify the subject amongst similar typed entities.
    /// Implementations should consider adding a `[StringLength]` of 20 or less.
    /// </summary>
    string Code { get; }

    /// <summary>
    /// A Title to display with the subject for single line cards and lists.
    /// Implementations should consider adding a `[StringLength]` of 50 characters or less.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// A Description for the subject to elaborate on details, may also be considered for markdown support.
    /// </summary>
    string Description { get; }
}
