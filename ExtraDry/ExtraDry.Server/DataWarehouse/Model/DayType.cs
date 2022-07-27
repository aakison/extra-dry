namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// Declares the type of day for a Date in the data warehouse.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DayType {
    
    /// <summary>
    /// A typical working day, such as Monday-Friday in the US, or Monday-Saturday in South Korea.
    /// For a 24/7 shift work environment, every day might be considered a workday.
    /// </summary>
    Workday,

    /// <summary>
    /// A weekend day that does not have work scheduled.
    /// </summary>
    Weekend,

    /// <summary>
    /// An ad-hoc day where work is not done for some holiday reason.
    /// </summary>
    Holiday,

}
