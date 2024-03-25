using ExtraDry.Server.Security;

namespace Sample.Components.Api.Security;

public class AuthorizationOptions
{
    public const string SectionName = "Authorization";

    public string Test { get; set; } = "Test";

    public Dictionary<string, List<Condition>> Conditions { get; init; } = [];

    public List<Policy> Policies { get; init; } = [];

    internal void Dump(ILogger logger)
    {
        logger.LogStaticInformation($"Authorization Options, {Conditions.Count} condtions:");
        logger.LogStaticInformation($"Test: {Test}");
        foreach(var condition in Conditions) {
            logger.LogStaticInformation($"Condition: {condition.Key}");
            foreach(var ruleValue in condition.Value) {
                logger.LogStaticInformation($"  Roles: [{string.Join(", ", ruleValue.Roles)}]");
                logger.LogStaticInformation($"  Claims: {string.Join(", ", ruleValue.Claims)}");
                foreach(var claim in ruleValue.Claims) {
                    logger.LogStaticInformation($"    {claim.Key}: {claim.Value}");
                }
                logger.LogStaticInformation($"  Attributes: {string.Join(", ", ruleValue.Attributes)}");
                foreach(var attribute in ruleValue.Attributes) {
                    logger.LogStaticInformation($"    {attribute.Key}: {attribute.Value}");
                }
            }
        }
    }
}

public class Policy : Condition
{
    public string? Name { get; set; }

    public List<string> Types { get; set; } = [];

    public List<AbacOperation> Operations { get; set; } = [];

    public List<string> Conditions { get; set; } = [];

}
