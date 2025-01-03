using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// During design time, the library needs access to the connection string and to be able to build a
/// context. This pattern allows EF Migrations to occur without being embedded in an application.
/// Additionally, this is used for the Unit Tests to create a SodDbContext with the correct config
/// settings. Because this is not production, a bad or missing connection string will cause a fake
/// name to be used.
/// </summary>
public class WarehouseContextFactory : IDesignTimeDbContextFactory<WarehouseContext>
{
    /// <summary>
    /// During creation, create a configuration that only accesses user-secrets for the connection
    /// string.
    /// </summary>
    public WarehouseContextFactory()
    {
        var builder = new ConfigurationBuilder();
        Configuration = builder.Build();
    }

    /// <summary>
    /// Create the DbContext.
    /// </summary>
    public WarehouseContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<WarehouseContext>();
        var connectionString = Configuration.GetConnectionString("WarehouseContext") ??
            @"Server=(localdb)\mssqllocaldb;Database=ExtraDryWarehouse;Trusted_Connection=True;";
        builder.UseSqlServer(connectionString);
        return new WarehouseContext(builder.Options);
    }

    private IConfigurationRoot Configuration { get; set; }
}
