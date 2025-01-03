namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior before they are restored.
/// </summary>
public interface IRestoringCallback
{
    /// <summary>
    /// Handling for the item that is done as it is being restored. This is called just before the
    /// object is restored by the Rule Engine. This is not intended to be called by user code.
    /// </summary>
    public Task OnRestoringAsync();
}

/// <summary>
/// The interface for entities that want to embellish the behavior after they are restored.
/// </summary>
public interface IRestoredCallback
{
    /// <summary>
    /// Handling for the item that is done as it is being restored. This is called just after the
    /// object is deleted by the Rule Engine. This is not intended to be called by user code.
    /// </summary>
    public Task OnRestoredAsync(RestoreResult result);
}

/// <summary>
/// The interface for entities that want to embellish the behavior before they are restored.
/// </summary>
public interface IRestoreCallbacks : IRestoringCallback, IRestoredCallback
{
}
