using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Models;

public class Model {

    [Filter]
    [JsonIgnore]
    public int Id { get; set; }

    [Filter(FilterType.Equals)]
    public string Name { get; set; } = string.Empty;

    [Filter(FilterType.StartsWith)]
    public string Soundex { get; set; } = string.Empty;

    [Filter]
    public ModelType Type { get; set; }

    public string Notes { get; set; } = string.Empty;
}
