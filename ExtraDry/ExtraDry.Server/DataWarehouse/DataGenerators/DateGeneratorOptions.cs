namespace ExtraDry.Server.DataWarehouse;

public class DateGeneratorOptions
{
    public DateOnly StartDate { get; set; } = new DateOnly(2000, 1, 1);

    public DateOnly EndDate { get; set; } = new DateOnly(DateTime.UtcNow.Year, 12, 31);

    public int FiscalYearEndingMonth {
        get => fiscalYearEndingMonth;
        set {
            if(value is < 1 or > 12) {
                throw new ArgumentOutOfRangeException(nameof(value), "Fiscal Year Ending Month is 1 indexed from January and must be between 1 and 12 inclusive.");
            }
            fiscalYearEndingMonth = value;
        }
    }

    private int fiscalYearEndingMonth = 12;

    public Func<DateOnly, DayType> DayTypesSelector { get; set; } = TrivialHolidaySelector;

    private static DayType TrivialHolidaySelector(DateOnly date)
    {
        if((date.Month == 12 && date.Day == 25) || (date.Month == 1 && date.Day == 1)) {
            return DayType.Holiday;
        }
        else if(date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) {
            return DayType.Weekend;
        }
        else {
            return DayType.Workday;
        }
    }
}
