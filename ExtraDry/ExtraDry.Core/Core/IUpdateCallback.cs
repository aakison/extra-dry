namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior when they are updated.
/// </summary>
public interface IUpdateCallback {

    /// <summary>
    /// Handling for the item that is done as it is being updated. This is not intended to be 
    /// called by user code and is automatically called by the RuleEngine during the 
    /// UpdateAsync method.
    /// </summary>
    public void OnUpdate();

}

/// <summary>
/// The async interface for entities that want to embellish the behavior when they are updated.
/// </summary>
public interface IUpdateAsyncCallback {

    /// <summary>
    /// Async handling for the item that is done as it is being updated. This is not intended to 
    /// be called by user code and is automatically called by the RuleEngine during the 
    /// UpdateAsync method.
    /// </summary>
    public Task OnUpdateAsync();

}
