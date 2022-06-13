namespace ExtraDry.Server.DataWarehouse;

internal static class StandardConversions {

    public static int DateTimeToId(DateTime dateTime) => CreateId(DateTimeToSequence(dateTime), DayType.Workday);

    public static int DateOnlyToSequence(DateOnly date) => DateTimeToSequence(date.ToDateTime(new TimeOnly(0)));
    
    public static int DateTimeToSequence(DateTime dateTime) => (dateTime - DateTime.UnixEpoch).Days;



    /// <summary>
    /// For a given date sequence and day type, creates a deterministic and monotonically increasing Id.
    /// </summary>
    public static int CreateId(int sequence, DayType dayType)
    {
        return 5 * sequence + (int)dayType;
    }



}
