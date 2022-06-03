namespace ExtraDry.Server.DataWarehouse;

public class DataFactoryOptions {

    /// <summary>
    /// The size of the batches that are fetched from the OLTP database per table.
    /// Keep size small enough to avoid too much database utilization and prevent locking.
    /// But, keep large enough to get batch performance improvements.
    /// </summary>
    public int BatchSize { 
        get => batchSize; 
        init {
            if(value < 1 || value > 10_000) {
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Valid batch sizes are between 1 and 10,000");
            }
            batchSize = value;
        }
    }
    private int batchSize = 100;

    /// <summary>
    /// Determines if migrations are automatically checked and applied each time the factory starts.
    /// </summary>
    public bool AutoMigrations { get; init; } = true;

}
