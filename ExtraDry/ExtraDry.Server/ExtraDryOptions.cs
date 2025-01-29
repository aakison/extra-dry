namespace ExtraDry.Server;

public class ExtraDryOptions
{
    /// <summary>
    /// When binding from configuration, this is the section name to use.
    /// </summary>
    public const string SectionName = "ExtraDry";

    public SortStabilization Stabilization { get; set; } = SortStabilization.PrimaryKey;
}
