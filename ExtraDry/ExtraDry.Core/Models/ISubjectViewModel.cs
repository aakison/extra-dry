namespace ExtraDry.Core;

/// <summary>
/// Represents the interface that defines the names of Subjects. Models can implement this or
/// ViewModel objects can implement this for models. Consider ISubjectViewModel`TModel instead to
/// use a ViewModel controller.
/// </summary>
public interface ISubjectViewModel : IListItemViewModel
{
    /// <summary>
    /// A Code that is used to uniquely identify the subject amongst similar typed entities.
    /// </summary>
    string Code { get; }

    /// <summary>
    /// A Title to display with the subject for single line cards and lists.
    /// </summary>
    string Subtitle { get; }

    /// <summary>
    /// A caption for presenting on a card with other information, should uniquely identify the
    /// entity when only a single line of text available.
    /// </summary>
    string Caption { get; }

    /// <summary>
    /// When not empty, a URL that indicates where a thumnbail of the subject can be downloaded.
    /// </summary>
    string Icon { get; }
}

/// <summary>
/// A ViewModel controller interface to extract subject information out of models.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public interface ISubjectViewModel<TModel> : IListItemViewModel<TModel>
{
    /// <inheritdoc cref="ISubjectViewModel.Code" />
    string Code(TModel item);

    /// <inheritdoc cref="ISubjectViewModel.Subtitle" />
    string Subtitle(TModel item);

    /// <inheritdoc cref="ISubjectViewModel.Caption" />
    string Caption(TModel item);

    /// <inheritdoc cref="ISubjectViewModel.Icon" />
    string Icon(TModel item);
}
