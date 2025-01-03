using ExtraDry.Server.DataWarehouse.Builder;
using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.DataWarehouse;

[JsonInterfaceConverter(typeof(DataGeneratorConverter))]
public interface IDataGenerator
{
    public Task<List<object>> GetBatchAsync(Table table, DbContext oltpContext, DbContext olapContext, ISqlGenerator sqlGenerator);

    public DateTime GetSyncTimestamp();
}
