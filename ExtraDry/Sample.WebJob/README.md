




// Today and Tomorrow:
//services.AddScoped<FinanceWarehouseContext>();
// Tomorrow:
//services.AddWarehouseContext<FinanceWarehouseContext>(options => {
//    options.NamingScheme = NamingScheme.KebabCase;
//});

//public class ContextlessWarehouseModel : WarehouseModel {

//    public ContextlessWarehouseModel() : base(typeof(SampleContext)) { }

//    protected override void OnCreating(WarehouseModelBuilder builder)
//    {
//        builder.Fact<Company>().Measure(e => e.AnnualRevenue).HasName("Big Bucks");
//        builder.Dimension<Date>().HasDateGenerator(options => {
//            options.StartDate = new DateOnly(2020, 1, 1);
//            options.EndDate = new DateOnly(DateTime.UtcNow.Year, 12, 31);
//            options.FiscalYearEndingMonth = 6;
//        });
//        builder.Dimension<Date>().Attribute(e => e.DayOfWeekName).IsIncluded(false);
//        builder.Dimension<Time>().HasTimeGenerator();
//    }

//}



//public class FinanceWarehouseContext : WarehouseModel<SampleContext> {

//    public FinanceWarehouseContext() : base("Finance") { }

//    protected override void OnCreating(WarehouseModelBuilder builder)
//    {
//        builder.Fact<Company>().Measure(e => e.AnnualRevenue).HasName("Big Bucks");
//        builder.Dimension<Date>().HasDateGenerator(options => {
//            options.StartDate = new DateOnly(2020, 1, 1);
//            options.EndDate = new DateOnly(DateTime.UtcNow.Year, 12, 31);
//            options.FiscalYearEndingMonth = 6;
//        });
//        builder.Dimension<Date>().Attribute(e => e.DayOfWeekName).IsIncluded(false);
//        builder.Dimension<Time>().HasTimeGenerator();
//    }

//}

