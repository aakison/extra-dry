namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior before they are created.
/// </summary>
public interface ICreatingCallback
{

    /// <summary>
    /// Callback handler for the item that is done as it is being created for the first time. This
    /// is called just after the object is initialized but before the Rule Engine creates the 
    /// object from an exemplar.  This is not intended to be called by user code.
    /// </summary>
    public Task OnCreatingAsync();

}

/// <summary>
/// The interface for entities that want to embellish the behavior after they are created.
/// </summary>
public interface ICreatedCallback
{

    /// <summary>
    /// Callback handler for the item that is done as it is being created for the first time. This
    /// is called just after the Rule Engine creates the object from an exemplar.  This is not 
    /// intended to be called by user code.
    /// </summary>
    public Task OnCreatedAsync();

}

/// <summary>
/// The interface for entities that want to embellish the behavior during creation.
/// </summary>
public interface ICreateCallbacks : ICreatingCallback, ICreatedCallback
{

}
