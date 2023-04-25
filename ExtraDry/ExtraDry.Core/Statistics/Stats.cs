namespace ExtraDry.Core;

/// <summary>
/// The type of statistics to apply to extract from this property.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Stats {

    /// <summary>
    /// Returns a summary of the frequency distribution of a given attribute, counting by unique 
    /// values.  Typically only apply to enums, but could apply to any column with a small set of 
    /// discrete values.
    /// </summary>
    Distribution = 1,

    // Placeholders
    ///// <summary>
    ///// For numeric data, such as age, where the values can be averaged and have meaning but lose
    ///// meaning when summed.  Provides statistical information on mean and standard deviation.
    ///// </summary>
    //Deviation = 2,

    ///// <summary>
    ///// For numeric data, such as salary, where the information has meaning as a total value.
    ///// Includes mean and standard deviation stats.
    ///// </summary>
    //Aggregate = 3,

}

