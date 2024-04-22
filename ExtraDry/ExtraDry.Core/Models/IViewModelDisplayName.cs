namespace ExtraDry.Core;

/// <summary>
/// Represents the interface that defines the display name of a model.
/// </summary>
public interface IViewModelDisplayName
{
    /// <summary>
    /// Given the model, return the entity display name
    /// </summary>
    /// <typeparam name="T">The type of the model</typeparam>
    /// <param name="model">The model</param>
    /// <returns>The display name</returns>
    public string DisplayName<T>(T model);
}
