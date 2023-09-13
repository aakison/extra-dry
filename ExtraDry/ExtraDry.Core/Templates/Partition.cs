using System.Xml.Serialization;

namespace ExtraDry.Core;

/// <summary>
/// Represents a logical partioning of data as determined by a template field.
/// This is used, for example, by Zuuse Capture to create a drilldown structure.
/// </summary>
public class Partition {

    
    public string Caption { get; set; }

    
    public string PartitionKey { get; set; }

    
    public string Display { get; set; }
}
