namespace Sample.Identity;


/// <summary>
/// The configuration settings for the app that are expected to found in environment variables or user-secrets.
/// In environment variables these are named as class:property, e.g. AppConfiguration:SupportEmail.
/// </summary>
public class AppConfiguration
{
    public AppConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
        configuration.GetSection(nameof(AppConfiguration)).Bind(this);
        ConnectionString = HostingModel switch {
            HostingModel.Kestrel => configuration.GetConnectionString("KestrelConnection"),
            HostingModel.IISExpress => configuration.GetConnectionString("IISExpressConnection"),
            HostingModel.Docker => configuration.GetConnectionString("DockerConnection"),
            _ => null,
        }
        ?? configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException($"Connection string not found, checked '{HostingModel}Connection' and 'DefaultConnection'.");

        ConnectionString = ConnectionString.Replace($"{{{nameof(IdentityAppPassword)}}}", IdentityAppPassword);
    }

    public string Secret { get; set; } = "";

    public string IdentityAppPassword { get; set; } = "";

    public HostingModel HostingModel { get; set; } = HostingModel.Kestrel;

    public string ConnectionString { get; set; } = "";

    private readonly IConfiguration configuration;
}

public enum HostingModel
{
    Kestrel,
    IISExpress,
    Docker,
}
