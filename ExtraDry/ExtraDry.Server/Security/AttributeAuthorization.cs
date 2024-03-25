using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraDry.Server.Security;

public class AttributeAuthorization(
    IAuthorizationService authorizationService,
    //IOptions<AuthorizationOptions> options,
    IHttpContextAccessor contextAccessor)
{

    public bool IsAuthorized(object target, AbacOperation operation)
    {
        return true;
    }

    public void AssertAuthorized(object target, AbacOperation operation)
    {
        if(IsAuthorized(target, operation) == false) {
            throw new UnauthorizedAccessException();
        }
    }
}

public class ConfigurationRequirement : IAuthorizationRequirement
{



}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AbacOperation
{
    Create,
    Read,
    Update,
    Delete,
    List,
    Aggregate,
    Execute,
}
