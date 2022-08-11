namespace ExtraDry.Core;

/// <summary>
/// Represents the interface that defines the names of Subjects.
/// Models can implement this or ViewModel objects can implement this for models.
/// Consider ISubjectViewModel`TModel instead to use a ViewModel controller.
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

/// <summary>
/// A ViewModel controller interface to extract subject information out of models.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public interface ISubjectViewModel<TModel>
{
    /// <inheritdoc cref="ISubjectViewModel.Code" />
    string Code(TModel item);

    /// <inheritdoc cref="ISubjectViewModel.Title" />
    string Title(TModel item);

    /// <inheritdoc cref="ISubjectViewModel.Subtitle" />
    string Subtitle(TModel item);

    /// <inheritdoc cref="ISubjectViewModel.Caption" />
    string Caption(TModel item);

    /// <inheritdoc cref="ISubjectViewModel.Thumbnail" />
    string Thumbnail(TModel item);

    /// <inheritdoc cref="ISubjectViewModel.Description" />
    string Description(TModel item);
}
