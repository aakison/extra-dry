namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior when they are deleted.
/// </summary>
public interface IDeleteCallback {

    /// <summary>
    /// Handling for the item that is done as it is being deleted. This is not intended to be 
    /// called by user code and is automatically called by the RuleEngine during the 
    /// DeleteAsync method.
    /// </summary>
    public void OnDelete(DeleteResult result);

}

/// <summary>
/// The async interface for entities that want to embellish the behavior when they are deleted.
/// </summary>
public interface IDeleteAsyncCallback {

    /// <summary>
    /// Async handling for the item that is done as it is being deleted. This is not intended to 
    /// be called by user code and is automatically called by the RuleEngine during the 
    /// DeleteAsync method.
    /// </summary>
    public Task OnDeleteAsync(DeleteResult result);

}
