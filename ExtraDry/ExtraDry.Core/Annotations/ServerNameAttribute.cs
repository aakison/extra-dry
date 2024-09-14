namespace ExtraDry.Core;

/// <summary>
/// Validates a property as being a valid server name, such as for URLs.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ServerNameAttribute : RegularExpressionAttribute
{
    public ServerNameAttribute() : base(@"[a-zA-Z0-9][a-zA-Z0-9.\-]*[a-zA-Z0-9]") { 
        ErrorMessage = "The server name must be a valid DNS name or IP address.";
    }
}
