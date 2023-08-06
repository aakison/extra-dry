namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior when they are updated.
/// </summary>
public interface IUpdateCallback {

    /// <summary>
    /// Callback handler for the item that is done as it is being updated. This is called just 
    /// before the object is updated by the Rule Engine.  This is not intended to be called by 
    /// user code.
    /// </summary>
    public Task OnUpdatingAsync();

    /// <summary>
    /// Callback handler for the item that is done as it is being updated. This is called just 
    /// after the object is updated by the Rule Engine.  This is not intended to be called by 
    /// user code.
    /// </summary>
    public Task OnUpdatedAsync();

}
