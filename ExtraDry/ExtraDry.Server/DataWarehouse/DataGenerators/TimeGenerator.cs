﻿using ExtraDry.Server.DataWarehouse.Builder;
using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.DataWarehouse;

public class TimeGenerator : IDataGenerator
{
    public async Task<List<object>> GetBatchAsync(Table table, DbContext oltpContext, DbContext olapContext, ISqlGenerator sqlGenerator)
    {
        var batch = new List<object>();

        var maxSql = sqlGenerator.SelectMaximum(table, table.KeyColumn.Name);
        var actualMax = await olapContext.Database.ExecuteScalerAsync(maxSql);

        var requiredMax = 24 * 60;
        if(requiredMax > actualMax) {
            var start = actualMax + 1;
            var end = requiredMax;
            for(int t = start; t < end; ++t) {
                batch.Add(new TimeDimension(new TimeOnly(t / 60, t % 60)));
            }
            return batch;
        }

        return batch;
    }

    public DateTime GetSyncTimestamp() => DateTime.UtcNow;
}
