namespace Sample.Components.Api.Security;

public class Condition
{
    public List<string> Roles { get; init; } = [];

    public Dictionary<string, string> Claims { get; init; } = [];


    public Dictionary<string, string> Attributes { get; init;} = [];
}

