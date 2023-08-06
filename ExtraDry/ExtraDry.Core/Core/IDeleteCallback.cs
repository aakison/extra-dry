namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior when they are deleted.
/// </summary>
public interface IDeleteCallback {

    /// <summary>
    /// Callback handler for the item that is done as it is being deleted. This is called just 
    /// before the object is deleted by the Rule Engine.  This is not intended to be called by 
    /// user code.
    /// </summary>
    /// <param name="action">The type of delete action, which can be changed in this callback.</param>
    public Task OnDeletingAsync(ref DeleteAction action);

    /// <summary>
    /// Callback handler for the item that is done as it is being deleted. This is called just 
    /// after the object is deleted by the Rule Engine.  This is not intended to be called by 
    /// user code.
    /// </summary>
    /// <param name="result">The result of delete action.</param>
    public Task OnDeletedAsync(DeleteResult result);

}
