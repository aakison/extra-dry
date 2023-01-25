namespace ExtraDry.Blazor;

/// <summary>
/// An interface not to be used by classes, but provides a target for `inheritdoc` XML comments.
/// Use these comments to ensure consistency across the entire framework.
/// </summary>
internal interface IComments {

    /// <summary>
    /// When an element is not selected, the text to display as a prompt for users.
    /// </summary>
    string Placeholder { get; set; }

    /// <summary>
    /// A key for an Icon in the component using the global themed Icon system.  Register keys for
    /// Icons as either fonts or images in the `Theme` component and then reference by key.
    /// </summary>
    string? Icon { get; set; }

}

/// <inheritdoc cref="IComments" />
internal interface IComments<TItem> {

    /// <summary>
    /// A list of items for the component.  The component may not render all items all of the time.
    /// Use when list is relatively small or is always known by the client application.  When the
    /// list is large and/or server driven consider `ItemsSource` instead.  `Items` and 
    /// `ItemsSource` are mutually exclusive.
    /// </summary>
    public IEnumerable<TItem>? Items { get; set; }

    /// <summary>
    /// An object that can provide the list of items for the component, typically only providing a
    /// subset of items that are required at any given time.  Use when the list is relatively large
    /// or when the list is only known to the server and is changing.  When the list is small or
    /// deterministic, consider using `Items` instead.  `Items` and `ItemsSource` are 
    /// mutually exclusive.
    /// </summary>
    IListService<TItem>? ItemsSource { get; set; }

    /// <summary>
    /// Provides a ViewModel for the component which is used to provide additional information
    /// about individual models in the components.  ExtraDry ViewModels are designed to be reused
    /// across multiple Models, separating logic and reducing memory pressure on the system.
    /// </summary>
    IListItemViewModel<TItem>? ViewModel { get; set; }

    /// <summary>
    /// The selected value of the component.  Use with two-way data binding.
    /// </summary>
    TItem? Value { get; set; }

    /// <summary>
    /// The changed event for `Value`.  Can be used directly, or preferably use indirectly as it
    /// supports two-way data binding.
    /// </summary>
    EventCallback<TItem> ValueChanged { get; set; }


}
