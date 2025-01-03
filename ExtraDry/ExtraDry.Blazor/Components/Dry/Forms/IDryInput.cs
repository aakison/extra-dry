namespace ExtraDry.Blazor;

/// <summary>
/// An interface for any component that can be used as a form input. This includes all the concrete
/// type components that are composited into the <see cref="IDryInput{T}" /> component.
/// </summary>
/// <typeparam name="T">The type of the Model that the input renders a property for.</typeparam>
public interface IDryInput<T>
    : IExtraDryComponent
    where T : class
{
    /// <summary>
    /// The Model that the input renders a property for.
    /// </summary>
    public T Model { get; set; }

    /// <summary>
    /// The PropertyDescription, taken from the <see cref="ViewModelDescription" /> which controls
    /// which property the component is for and the details about how it is rendered. Can be
    /// retrieved from one of the 'For' factory methods on PropertyDescription.
    /// </summary>
    PropertyDescription Property { get; set; }

    /// <summary>
    /// Event for handling changes to the input.
    /// </summary>
    EventCallback<ChangeEventArgs> OnChange { get; set; }

    /// <inheritdoc cref="Blazor.EditMode" />
    EditMode EditMode { get; set; }
}
