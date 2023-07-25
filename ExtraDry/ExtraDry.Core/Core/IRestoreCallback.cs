namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior when they are restored.
/// </summary>
public interface IRestoreCallback {

    /// <summary>
    /// Handling for the item that is done as it is being restored. This is not intended to be 
    /// called by user code and is automatically called by the RuleEngine during the 
    /// DeleteAsync method.
    /// </summary>
    public void OnRestore(RestoreResult result);

}

/// <summary>
/// The async interface for entities that want to embellish the behavior when they are restored.
/// </summary>
public interface IRestoreAsyncCallback {

    /// <summary>
    /// Async handling for the item that is done as it is being restored. This is not intended to 
    /// be called by user code and is automatically called by the RuleEngine during the 
    /// DeleteAsync method.
    /// </summary>
    public Task OnRestoreAsync(RestoreResult result);

}
