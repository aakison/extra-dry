using ExtraDry.Server.DataWarehouse;
using Sample.Data;
using Sample.Shared;

namespace Sample.WebJob;

public class SampleWarehouseModel : WarehouseModel<SampleContext> {

    protected override void OnCreating(WarehouseModelBuilder builder)
    {
        builder.Fact<Company>().Measure(e => e.AnnualRevenue).HasName("Big Bucks");
        builder.Dimension<Date>().HasDateGenerator(options => {
            options.StartDate = new DateOnly(2020, 1, 1);
            options.EndDate = new DateOnly(DateTime.UtcNow.Year, 12, 31);
            options.FiscalYearEndingMonth = 6;
        });
        builder.Dimension<Date>().Attribute(e => e.DayOfWeekName).IsIncluded(false);
        builder.Dimension<Time>().HasTimeGenerator();
    }

}
