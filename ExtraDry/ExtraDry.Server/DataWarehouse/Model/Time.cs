using System.Globalization;

namespace ExtraDry.Server.DataWarehouse;

[DimensionTable]
public class Time {
    
    public Time(TimeOnly time)
    {
        Value = new TimeOnly(time.Hour, time.Minute);
        Id = 60 * Value.Hour + Value.Minute;
    }

    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [Attribute("Time")]
    public TimeOnly Value { get; set; }

    /// <summary>
    /// Hour in 24 hour time format, from 0 to 23
    /// </summary>
    [Attribute("Hour 24")]
    public int Hour24 => Value.Hour;

    /// <summary>
    /// Hour in 12 hour time format, use with AM/PM
    /// </summary>
    [Attribute("Hour 12")]
    public int Hour12 => (Value.Hour + 11) % 12 + 1;

    /// <summary>
    /// The minutes from the top of the hour.
    /// </summary>
    public int Minute => Value.Minute;

    /// <summary>
    /// The quarter of the hour for histograms.
    /// </summary>
    public int QuarterHour => 15 * (Value.Minute / 15);

    /// <summary>
    /// The hour divide in twelths, for five-minute accuracy histograms.
    /// </summary>
    public int TwelthHour => 5 * (Value.Minute / 5);

    [StringLength(2)]
    public string Meridian => Value.ToString("tt", CultureInfo.CurrentCulture);

    [StringLength(5)]
    [Attribute("24 Hour Time Name")]
    public string Time24Name => Value.ToString("HH:mm", CultureInfo.CurrentCulture);

    [StringLength(8)]
    [Attribute("12 Hour Time Name")]
    public string Time12Name => Value.ToString("hh:mm tt", CultureInfo.CurrentCulture);

}
