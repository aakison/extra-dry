using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.DataWarehouse;

public class WarehouseContext : DbContext {

    public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) { }

    
    public DbSet<DataTableSync> TableSyncs { get; set; } = null!;

}
