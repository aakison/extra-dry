using ExtraDry.Server.DataWarehouse.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExtraDry.Server.DataWarehouse;

public class DataGeneratorConverter : JsonConverter<IDataGenerator> {

    public override IDataGenerator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IDataGenerator value, JsonSerializerOptions options)
    {
        var type = value.GetType();
        JsonSerializer.Serialize(writer, value, type, options);
    }
}

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
public class JsonInterfaceConverterAttribute : JsonConverterAttribute {
    public JsonInterfaceConverterAttribute(Type converterType)
        : base(converterType)
    {
    }
}

[JsonInterfaceConverter(typeof(DataGeneratorConverter))]
public interface IDataGenerator {

    public Task<List<object>> GetBatchAsync(Table table, DbContext oltpContext, DbContext olapContext, ISqlGenerator sqlGenerator);

    public DateTime GetSyncTimestamp();

}

public class DateGeneratorOptions {

    public DateOnly StartDate { get; set; } = new DateOnly(2000, 1, 1);

    public DateOnly EndDate { get; set; } = new DateOnly(DateTime.UtcNow.Year, 12, 31);

    public int FiscalYearEndingMonth {
        get => fiscalYearEndingMonth;
        set {
            if(value < 1 || value > 12) {
                throw new ArgumentOutOfRangeException(nameof(value), "Fiscal Year Ending Month is 1 indexed from January and must be between 1 and 12 inclusive.");
            }
            fiscalYearEndingMonth = value;
        }
    }
    private int fiscalYearEndingMonth = 12;

    public Func<DateOnly, IEnumerable<DayType>> DayTypesSelector { get; set; } = TrivialHolidaySelector;

    private static IEnumerable<DayType> TrivialHolidaySelector(DateOnly date) {
        if((date.Month == 12 && date.Day == 25) || (date.Month == 1 && date.Day == 1)) {
            yield return DayType.Holiday;
        }
        else if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) {
            yield return DayType.Weekend;
        }
        else {
            yield return DayType.Workday;
        }
    }

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

        var minSql = sql.SelectMinimum(table, nameof(Date.Sequence));
        var actualMin = await ExecuteScalerAsync(olap.Database, minSql);
        var requiredMin = DateToInt(StartDate);
        if(requiredMin < actualMin) {
            // Earlier dates are required, ensure they're added in decreasing order, reverses typical for loop.
            var start = actualMin - 1;
            var end = Math.Max(requiredMin, start - 100);
            for(int d = start; d >= end; --d) {
                AddDatesToBatch(batch, d);
            }
            return batch;
        }

        var maxSql = sql.SelectMaximum(table, nameof(Date.Sequence));
        var actualMax = await ExecuteScalerAsync(olap.Database, maxSql);

        var requiredMax = DateToInt(EndDate);
        if(requiredMax > actualMax) {
            var start = Math.Max(DateToInt(StartDate), actualMax + 1);
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
        foreach(var day in Options.DayTypesSelector(date)) {
            batch.Add(new Date(d, day) { FiscalYearEndingMonth = FiscalYearEndingMonth });
        }
    }

    // Not part of EF any more, need to hack it.  Might want to promote to an extension method if needed again.
    private static async Task<int> ExecuteScalerAsync(DatabaseFacade facade, string sql)
    {
        using var cmd = facade.GetDbConnection().CreateCommand();
        cmd.CommandText = sql;
        cmd.CommandType = System.Data.CommandType.Text;
        if(cmd.Connection!.State != System.Data.ConnectionState.Open) {
            cmd.Connection.Open();
        }
        var val = await cmd.ExecuteScalarAsync();
        return (val as int?) ?? -1;
    }

    public DateTime GetSyncTimestamp() => DateTime.UtcNow;

    private void RefreshOptions() => OptionsBuilder?.Invoke(Options);

    private static int DateToInt(DateOnly date) => (date.ToDateTime(new TimeOnly(0)) - DateTime.UnixEpoch).Days;

    private static DateOnly IntToDate(int days) => new DateOnly(1970, 1, 1).AddDays(days);

    private DateGeneratorOptions Options { get; set; } = new DateGeneratorOptions();

    private Action<DateGeneratorOptions>? OptionsBuilder { get; set; }

}
