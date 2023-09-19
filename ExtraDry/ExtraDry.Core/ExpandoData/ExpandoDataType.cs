namespace ExtraDry.Core;

// TODO: Refine this list
public enum ExpandoDataType {

    //
    // Summary:
    //     Represents an instant in time, expressed as a date and time of day.
    DateTime = 1,
    //
    // Summary:
    //     Represents a date value.
    Date = 2,
    //
    // Summary:
    //     Represents a time value.
    Time = 3,
    //
    // Summary:
    //     Represents a continuous time during which an object exists.
    Duration = 4,
    //
    // Summary:
    //     Represents a phone number value.
    PhoneNumber = 5,
    //
    // Summary:
    //     Represents a currency value.
    Currency = 6,
    //
    // Summary:
    //     Represents text that is displayed.
    Text = 7,
    //
    // Summary:
    //     Represents an email address.
    EmailAddress = 10,
    //
    // Summary:
    //     Represents a URL value.
    Url = 12,
    //
    // Summary:
    //     Represents a URL to an image.
    ImageUrl = 13,
    //
    // Summary:
    //     Represents a postal code.
    PostalCode = 15,

    Boolean = 16, // e.g. for checkboxes

    // Link concepts??? TODO: Consider...
    Number = 20,
    User = 21,
    Asset = 22,
    WorkOrder = 22,
    PlannedMaintenance = 23,
    
}
