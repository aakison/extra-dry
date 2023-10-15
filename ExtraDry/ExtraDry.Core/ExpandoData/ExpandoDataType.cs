namespace ExtraDry.Core;

/// <summary>
/// The base data type for an expansion field.  Closely follows the JSON data types, but adds
/// DateTime as a primitive type that is not present in JSON.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExpandoDataType {

    /// <summary>
    /// Represents an instant in time, expressed as a date and time of day.
    /// </summary>
    /// <remarks>
    /// Extracted from JSON iff Text and exactly matches "yyyy-mm-ddThh:mm:ss[.fffffff]Z".
    /// Base for date related subtypes such as Date, Time, DateTime, DateTimeOffset
    /// </remarks>
    DateTime,

    /// <summary>
    /// Represents text that is displayed.
    /// </summary>
    /// <remarks>
    /// Base for text related sub types, such as PhoneNumber, Email, Url, etc.
    /// </remarks>
    Text, 

    /// <summary>
    /// Represents True/False
    /// </summary>
    Boolean,

    /// <summary>
    /// Represents a Number.
    /// </summary>
    /// <remarks>
    /// Base for number related sub types, such as Integer, Real, Currency, etc.
    /// </remarks>
    Number, 
    
}
