using ExtraDry.Server.DataWarehouse.Builder;

namespace ExtraDry.Server.DataWarehouse;

public static class TimeGeneratorBuilderExtensions {

    public static DimensionTableBuilder<T> HasTimeGenerator<T>(this DimensionTableBuilder<T> source) where T : Time
    {
        source.HasGenerator(new TimeGenerator());
        return source;
    }

}
