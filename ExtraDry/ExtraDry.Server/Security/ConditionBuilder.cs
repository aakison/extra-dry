namespace ExtraDry.Server.Security;

public class ConditionBuilder
{
    public ConditionBuilder(AbacOptions options, string name)
    {
        Options = options;
        Name = name;
        if(Options.Conditions.TryGetValue(name, out var condition)) {
            Condition = condition;
        }
        else {
            Condition = new AbacCondition();
            Options.Conditions.Add(name, Condition);
        }
    }

    public ConditionBuilder RequiresRoles(params string[] roles)
    {
        foreach(var role in roles) {
            if(!Condition.Roles.Contains(role)) {
                Condition.Roles.Add(role);
            }
        }
        return this;
    }

    /// <summary>
    /// Creates a required claim for this condition. If the claim already exists with a different value expression, an exception is thrown. If it already exists with the same value expression, nothing happens.
    /// </summary>
    /// <param name="name">The name of the claim in the JWT access token.</param>
    /// <param name="valueExpression">
    /// The value expression for the claim.
    ///   - "literal": A literal value, such as "admin" or "user".
    ///   - "{user.expr}": A value from the IClaimsPrincipal with key "expr".
    ///   - "{route.expr}": A value from the RouteValueDictionary with key "expr".
    ///   - "{resource.expr}": A value from the deserialized HttpRequest.Body with key "expr", sourced using reflection.  Available with ABAC only.
    ///   - "{attribute.expr}": A value from the deserialized HttpRequest.Body with key "expr", sourced using IAttribute interface.  Available with ABAC only.
    /// </param>
    /// <returns>The current ConditionBuilder instance.</returns>
    /// <exception cref="DryException">Thrown if the claim already exists with a different value expression.</exception>
    public ConditionBuilder RequiresClaim(string name, string valueExpression)
    {
        if(Condition.Claims.TryGetValue(name, out var _)) {
            if(Condition.Claims[name] != valueExpression) {
                throw new DryException($"Claim {name} already exists in condition {Name} with a different value expression.");
            }
        }
        else {
            Condition.Claims.Add(name, valueExpression);
        }
        return this;
    }

    private AbacOptions Options { get; set; }

    private string Name { get; set; }

    private AbacCondition Condition { get; set; }
}
