namespace ExtraDry.Core;

/// <summary>
/// The interface for entities that want to embellish the behavior when they are created.
/// </summary>
public interface ICreateCallback {

    /// <summary>
    /// Handling for the item that is done as it is being created for the first time. This is not
    /// intended to be called by user code and is automatically called by the RuleEngine during the
    /// CreateAsync method.
    /// </summary>
    public void OnCreate();

}

/// <summary>
/// The interface for entities that want to embellish the async behavior when they are created.
/// </summary>
public interface ICreateAsyncCallback
{

    /// <summary>
    /// Async handling for the item that is done as it is being created for the first time. This is
    /// not intended to be called by user code and is automatically called by the RuleEngine during 
    /// the CreateAsync method.
    /// </summary>
    public Task OnCreateAsync();

}
