namespace ExtraDry.Server.Security;

public class AbacPolicy
{
    public string? Name { get; set; }

    public List<string> Types { get; set; } = [];

    public List<AbacOperation> Operations { get; set; } = [];

    public List<string> Conditions { get; set; } = [];

}
