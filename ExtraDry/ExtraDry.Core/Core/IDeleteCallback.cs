namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior when they are deleted.
/// </summary>
public interface IDeleteCallback {

    /// <summary>
    /// Handling that allows an entity to prepare for a deletion to be performed.  
    /// </summary>
    /// <param name="action">The type of delete action, which can be changed in this callback.</param>
    public void OnDeleting(DeleteAction action);

    /// <summary>
    /// Handling for the item that is done as it is being deleted. This is not intended to be 
    /// called by user code and is automatically called by the RuleEngine during the 
    /// DeleteAsync method.
    /// </summary>
    public void OnDeleted(DeleteResult result);

}

/// <summary>
/// The async interface for entities that want to embellish the behavior when they are deleted.
/// </summary>
public interface IDeleteAsyncCallback {

    /// <summary>
    /// Async handling that allows an entity to prepare for a deletion to be performed.  
    /// </summary>
    /// <param name="action">The type of delete action, which can be changed in this callback.</param>
    public Task OnDeletingAsync(DeleteAction action);

    /// <summary>
    /// Async handling for the item that is done as it is being deleted. This is not intended to 
    /// be called by user code and is automatically called by the RuleEngine during the 
    /// DeleteAsync method.
    /// </summary>
    public Task OnDeletedAsync(DeleteResult result);

}
