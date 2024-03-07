using ExtraDry.Server.Agents;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Sample.Components.Agent;

public class RabbitMQOptions 
{
    [StringLength(255)]
    [RegularExpression(@"[a-zA-Z0-9.\-]+")]
    public string Server { get; set; } = "localhost";

    [Required]
    public string VirtualHost { get; set; } = "/";

    [Required]
    public string Username { get; set; } = "guest";

    [Secret]
    [Required]
    public string Password { get; set; } = "password";

    [Range(1, 65536)]
    public int AdminPort { get; set; } = 15672;

    public string AdminUrl => $"http://{Username}:{Password}@{Server}:{AdminPort}{VirtualHost}";

}
