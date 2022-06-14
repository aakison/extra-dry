using ExtraDry.Server.DataWarehouse.Builder;

namespace ExtraDry.Server.DataWarehouse;

public static class DateGeneratorBuilderExtensions {

    public static DimensionTableBuilder<T> HasDateGenerator<T>(this DimensionTableBuilder<T> source, Action<DateGeneratorOptions>? options = null) where T : Date
    {
        source.HasGenerator(new DateGenerator(options));
        return source;
    }

}
