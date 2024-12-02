namespace ExtraDry.Server.Security;

public class RbacPolicyBuilder
{
    public RbacPolicyBuilder(AbacOptions options, string name)
    {
        Options = options;
        var policy = Options.Policies.SingleOrDefault(e => e.Name == name);
        if(policy == null) {
            policy = new AbacPolicy { Name = name };
            Options.Policies.Add(policy);
        }
        Policy = policy;
    }

    public RbacPolicyBuilder WithAnyCondition(params string[] conditionNames)
    {
        foreach(var conditionName in conditionNames) {
            if(!Policy.Conditions.Contains(conditionName)) {
                Policy.Conditions.Add(conditionName);
            }
        }
        return this;
    }

    private AbacOptions Options { get; set; }

    private AbacPolicy Policy { get; set; }
}
