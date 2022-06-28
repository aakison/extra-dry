namespace ExtraDry.Server.DataWarehouse;

internal static class StandardConversions {

    public static int DateOnlyToSequence(DateOnly date) => DateTimeToSequence(date.ToDateTime(new TimeOnly(0)));
    
    public static int DateTimeToSequence(DateTime dateTime) => (dateTime - DateTime.UnixEpoch).Days;

}
