namespace ExtraDry.Server.DataWarehouse;

public interface IDataGenerator {

    public Task<List<object>> GetBatch();

    public DateTime GetSyncTimestamp();

}

public class DateGenerator : IDataGenerator {
    
    public Task<List<object>> GetBatch()
    {
    }

    public DateTime GetSyncTimestamp() => DateTime.UtcNow;

}
