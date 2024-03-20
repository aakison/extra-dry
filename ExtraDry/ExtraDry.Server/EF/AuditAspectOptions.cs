namespace ExtraDry.Server.EF;

/// <summary>
/// Options for configuring the <see cref="AuditAspect"/>.  Use with the 
/// <see cref="ServiceCollectionExtensions.AddAuditAspect(IServiceCollection, Action{AuditAspectOptions})"/>
/// extension method during startup.
/// </summary>
public class AuditAspectOptions
{

    /// <summary>
    /// The name of the claim that contains the username.  Defaults to 'nameidentifier'.
    /// The matching logic for the claim can be configured by <see cref="StrictClaimMatch"/>.
    /// </summary>
    public string UsernameClaim { get; set; } = "nameidentifier";

    /// <summary>
    /// Indicates if the claim must match exactly or if it can match the stem of the claim.
    /// This simplifies the claim matching process allowing 'nameidentifeir' to match 
    /// 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'.
    /// </summary>
    public bool StrictClaimMatch { get; set; } 

    /// <summary>
    /// If the user claims do not match the indicated claim, the value for the username in 
    /// the audit.
    /// </summary>
    public string UsernameDefault { get; set; } = "anonymous";

}
