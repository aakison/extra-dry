namespace ExtraDry.Core;

/// <summary>
/// An interface for objects that use security attributes instead of type properties for 
/// ABAC authorization.  See <cref name="AbacAuthorization" />.
/// </summary>
public interface IAttributed
{

    /// <summary>
    /// The attributes that define the object for authorization purposes.
    /// </summary>
    public Dictionary<string, string> Attributes { get; set; }

}

