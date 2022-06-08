using ExtraDry.Server.DataWarehouse.Builder;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.DataWarehouse;

public interface IDataGenerator {

    public Task<List<object>> GetBatchAsync(DbContext oltpContext, DbContext olapContext, ISqlGenerator sqlGenerator);

    public DateTime GetSyncTimestamp();

}

public class DateGeneratorOptions {

    public DateOnly StartDate { get; set; } = new DateOnly(2000, 1, 1);

    public DateOnly EndDate { get; set; } = new DateOnly(DateTime.UtcNow.Year, 12, 31);

}

public static class DateGeneratorBuilderExtensions {

    public static DimensionTableBuilder<T> HasDateGenerator<T>(this DimensionTableBuilder<T> source, Action<DateGeneratorOptions>? options = null) where T : Date
    {
        source.HasGenerator(new DateGenerator(options));
        return source;
    }

}

public class DateGenerator : IDataGenerator {

    public DateGenerator(Action<DateGeneratorOptions>? options = null)
    {
        OptionsBuilder = options;
    }

    public DateOnly StartDate => Options.StartDate;

    public DateOnly EndDate => Options.EndDate;

    public async Task<List<object>> GetBatchAsync(DbContext oltp, DbContext olap, ISqlGenerator sql)
    {
        RefreshOptions();

        var dbMin = await olap.Database.ExecuteSqlRawAsync(sql.SelectMinimumKey(table));
        var dbMax = await olap.Database.ExecuteSqlRawAsync(sql.SelectMaximumKey(table));


        var batch = new List<object>();
        for(int i = 0; i < 100; ++i) {
            var date = new Date { Id = i, Value = IntToDate(i) };
            batch.Add(date);
        }
        return batch;
    }

    public DateTime GetSyncTimestamp() => DateTime.UtcNow;

    private void RefreshOptions() => OptionsBuilder?.Invoke(Options);

    private static int DateToInt(DateOnly date) => (date.ToDateTime(new TimeOnly(0)) - DateTime.UnixEpoch).Days;

    private static DateOnly IntToDate(int days) => new DateOnly(1970, 1, 1).AddDays(days);

    private DateGeneratorOptions Options { get; set; } = new DateGeneratorOptions();

    private Action<DateGeneratorOptions>? OptionsBuilder { get; set; }

}
