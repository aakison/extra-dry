namespace ExtraDry.Core;

/// <summary>
/// Represents the interface that defines the entity's caption for form.
/// </summary>
public interface IViewModelCaption
{
    /// <summary>
    /// Given the model, return the entity caption
    /// </summary>
    /// <typeparam name="T">The type of the model</typeparam>
    /// <param name="model">The model</param>
    /// <returns>The caption</returns>
    public string Caption<T>(T model);
}
