namespace ExtraDry.Server;

public class ExtraDryOptions {

    /// <summary>
    /// When binding from configuration, this is the section name to use.
    /// </summary>
    public const string SectionName = "ExtraDry";

    public string? ForbiddenTitle { get; set; }

    public string? ForbiddenMessage { get; set; }

    public string? UnauthorizedTitle { get; set; }

    public string? UnauthorizedMessage { get; set; }

    public SortStabilization Stabilization { get; set; } = SortStabilization.PrimaryKey;
}
