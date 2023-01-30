﻿namespace ExtraDry.Core;

/// <summary>
/// Represents the interface that defines the display titles for list items.
/// Models can implement this or ViewModel objects can implement this for models.
/// Consider IListItemViewModel`TModel instead to use a ViewModel controller.
/// </summary>
public interface IListItemViewModel {

    /// <summary>
    /// A Title to display with the subject for single line cards and lists.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// A Description for the subject to elaborate on details, may also be considered for markdown support.
    /// </summary>
    string Description { get; }

}

/// <summary>
/// A ViewModel controller interface to extract titles for list items out of models.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public interface IListItemViewModel<TModel>
{
    /// <inheritdoc cref="IListItemViewModel.Title" />
    string Title(TModel item);

    /// <inheritdoc cref="IListItemViewModel.Description" />
    string Description(TModel item);

}

public interface IGroupingViewModel<TModel>
{
    /// <summary>
    /// If provided, creates groupings for items in the list.
    /// </summary>
    string Group(TModel item);

    /// <summary>
    /// If provided, overrides the sort order for the group.  Useful for changing the order of
    /// groups that need to override the alphabetic sort, such as "Other" being the last group.
    /// </summary>
    string GroupSort(TModel item);

}