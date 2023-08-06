namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior when they are created.
/// </summary>
public interface ICreateCallback {

    /// <summary>
    /// Callback handler for the item that is done as it is being created for the first time. This
    /// is called just after the object is initialized but before the Rule Engine creates the 
    /// object from an exemplar.  This is not intended to be called by user code.
    /// </summary>
    public Task OnCreatingAsync();

    /// <summary>
    /// Callback handler for the item that is done as it is being created for the first time. This
    /// is called just after the Rule Engine creates the object from an exemplar.  This is not 
    /// intended to be called by user code.
    /// </summary>
    public Task OnCreatedAsync();

}
