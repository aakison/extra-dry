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
