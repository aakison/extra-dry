namespace ExtraDry.Core;

public enum ExpandoDataType {
    /// <summary>
    /// Represents an instant in time, expressed as a date and time of day.
    /// </summary>
    DateTime = 1,

    /// <summary>
    /// Represents a date value.
    /// </summary>
    Date = 2,

    /// <summary>
    /// Represents a time value.
    /// </summary>
    Time = 3,

    /// <summary>
    /// Represents a continuous time during which an object exists.
    /// </summary>
    Duration = 4,

    /// <summary>
    /// Represents a phone number value.
    /// </summary>
    PhoneNumber = 5,

    /// <summary>
    /// Represents a currency value.
    /// </summary>
    Currency = 6, // EUR 45.50

    /// <summary>
    /// Represents text that is displayed.
    /// </summary>
    Text = 7,

    /// <summary>
    /// Represents True/False
    /// </summary>
    Boolean = 8,

    /// <summary>
    /// Represents a Number.
    /// </summary>
    Number = 9,
    
}
