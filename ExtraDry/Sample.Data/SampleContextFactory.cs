#nullable enable

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Sample.Data;

/// <summary>
/// During design time, the library needs access to the connection string and to be able to build a context.
/// This pattern allows EF Migrations to occur without being embedded in an application.
/// Additionally, this is used for the Unit Tests to create a SodDbContext with the correct config settings.
/// Because this is not production, a bad or missing connection string will cause a fake name to be used.
/// </summary>
public class SampleDbContextFactory : IDesignTimeDbContextFactory<SampleContext> {

    /// <summary>
    /// During creation, create a configuration that only accesses user-secrets for the connection string.
    /// </summary>
    public SampleDbContextFactory()
    {
        var builder = new ConfigurationBuilder();
        Configuration = builder.Build();
    }

    /// <summary>
    /// Create the DbContext.
    /// </summary>
    public SampleContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SampleContext>();
        var connectionString = Configuration.GetConnectionString("SampleContext") ??
            @"Server=(localdb)\mssqllocaldb;Database=ExtraDrySample;Trusted_Connection=True;";
        builder.UseSqlServer(connectionString, config => config.UseHierarchyId());
        return new SampleContext(builder.Options, []);
    }

    private IConfigurationRoot Configuration { get; set; }
}
