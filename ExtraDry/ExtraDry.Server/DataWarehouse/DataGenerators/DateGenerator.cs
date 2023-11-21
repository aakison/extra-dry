using ExtraDry.Server.DataWarehouse.Builder;
using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.DataWarehouse;

public class DateGenerator : IDataGenerator {

    public DateGenerator(Action<DateGeneratorOptions>? options = null)
    {
        OptionsBuilder = options;
        RefreshOptions();
    }

    [JsonIgnore]
    public DateOnly StartDate => Options.StartDate;

    [JsonIgnore] 
    public DateOnly EndDate => Options.EndDate;

    public int FiscalYearEndingMonth => Options.FiscalYearEndingMonth;

    public async Task<List<object>> GetBatchAsync(Table table, DbContext oltp, DbContext olap, ISqlGenerator sql)
    {
        RefreshOptions();
        var batch = new List<object>();

        var minSql = sql.SelectMinimum(table, table.KeyColumn.Name);
        var actualMin = await olap.Database.ExecuteScalerAsync(minSql);
        var requiredMin = StandardConversions.DateOnlyToSequence(StartDate);
        if(requiredMin < actualMin) {
            // Earlier dates are required, ensure they're added in decreasing order, reverses typical for loop.
            var start = actualMin - 1;
            var end = Math.Max(requiredMin, start - 100);
            for(int d = start; d >= end; --d) {
                AddDatesToBatch(batch, d);
            }
            return batch;
        }

        var maxSql = sql.SelectMaximum(table, table.KeyColumn.Name);
        var actualMax = await olap.Database.ExecuteScalerAsync(maxSql);

        var requiredMax = StandardConversions.DateOnlyToSequence(EndDate);
        if(requiredMax > actualMax) {
            var start = Math.Max(StandardConversions.DateOnlyToSequence(StartDate), actualMax + 1);
            var end = Math.Min(requiredMax, start + 100);
            for(int d = start; d < end; ++d) {
                AddDatesToBatch(batch, d);
            }
            return batch;
        }

        return batch;
    }

    private void AddDatesToBatch(List<object> batch, int d)
    {
        var date = IntToDate(d);
        var day = Options.DayTypesSelector(date);
        batch.Add(new DateDimension(d, day) { FiscalYearEndingMonth = FiscalYearEndingMonth });
    }

    public DateTime GetSyncTimestamp() => DateTime.UtcNow;

    private void RefreshOptions() => OptionsBuilder?.Invoke(Options);

    private static DateOnly IntToDate(int days) => new DateOnly(1970, 1, 1).AddDays(days);

    private DateGeneratorOptions Options { get; set; } = new DateGeneratorOptions();

    private Action<DateGeneratorOptions>? OptionsBuilder { get; set; }

}
