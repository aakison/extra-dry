namespace ExtraDry.Core;

/// <summary>
/// Represents the interface that defines the names of Subjects.
/// </summary>
public interface ISubjectViewModel {

    /// <summary>
    /// A Code that is used to uniquely identify the subject amongst similar typed entities.
    /// </summary>
    string Code { get; }

    /// <summary>
    /// A Title to display with the subject for single line cards and lists.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// A Title to display with the subject for single line cards and lists.
    /// </summary>
    string Subtitle { get; }

    /// <summary>
    /// A caption for presenting on a card with other information, 
    /// should uniquely identify the entity when only a single line of text available.
    /// </summary>
    string Caption { get; }

    /// <summary>
    /// When not empty, a URL that indicates where a thumnbail of the subject can be downloaded.
    /// </summary>
    string Thumbnail { get; }

    /// <summary>
    /// A Description for the subject to elaborate on details, may also be considered for markdown support.
    /// </summary>
    string Description { get; }

}
