using ExtraDry.Server.Agents;

namespace Sample.Components.Agent;

public class ApiOptions
{
    [Required, ServerName]
    public string Server { get; set; } = "localhost";

    [Required, Range(1, 65536)]
    public int Port { get; set; } = 40443;

    [Required, Base64String]
    [Secret]
    public string Jwt { get; set; } = "JWT token with 'agent' role assigned, inject with user-secrets or environment variable.";

    public string BearerToken => $"Bearer {Jwt}";
}
