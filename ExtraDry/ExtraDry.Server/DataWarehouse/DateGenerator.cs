namespace ExtraDry.Server.DataWarehouse;

public interface IDataGenerator {

    public Task<List<object>> GetBatchAsync();

    public DateTime GetSyncTimestamp();

}

public class DateGenerator : IDataGenerator {

    public Task<List<object>> GetBatchAsync()
    {
        var batch = new List<object>();
        for(int i = 0; i < 100; ++i) {
            var date = new Date { Id = i, Value = IntToDate(i) };
            batch.Add(date);
        }
        return Task.FromResult(batch);
    }

    public DateTime GetSyncTimestamp() => DateTime.UtcNow;

    // Perhaps a IDateIdGenerator?
    private static int DateToInt(DateOnly date) => (date.ToDateTime(new TimeOnly(0)) - DateTime.UnixEpoch).Days;

    private static DateOnly IntToDate(int days) => new DateOnly(1970, 1, 1).AddDays(days);

}
