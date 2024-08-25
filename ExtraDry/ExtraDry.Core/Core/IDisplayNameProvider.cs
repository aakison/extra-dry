namespace ExtraDry.Core;

/// <summary>
/// The interface for resolving a name from an id for display purposes.
/// </summary>
public interface IDisplayNameProvider
{
    public Task<string> ResolveDisplayNameAsync(string user);
}
