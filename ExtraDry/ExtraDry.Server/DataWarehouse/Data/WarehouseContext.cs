using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.DataWarehouse;

public class WarehouseContext(
    DbContextOptions<WarehouseContext> options)
    : DbContext(options)
{
    public DbSet<DataTableSync> TableSyncs { get; set; } = null!;
}
