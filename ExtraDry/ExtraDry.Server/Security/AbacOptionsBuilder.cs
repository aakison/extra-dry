namespace ExtraDry.Server.Security;

public class AbacOptionsBuilder(AbacOptions options)
{
    public ConditionBuilder AddCondition(string conditionName)
    {
        var builder = new ConditionBuilder(options, conditionName);
        return builder;
    }

    public RbacPolicyBuilder AddRbacPolicy(string policyName)
    {
        var builder = new RbacPolicyBuilder(options, policyName);
        return builder;
    }
}
