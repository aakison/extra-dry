namespace ExtraDry.Server;

public class ExtraDryOptions {

    public string? ForbiddenTitle { get; set; }

    public string? ForbiddenMessage { get; set; }

    public string? UnauthorizedTitle { get; set; }

    public string? UnauthorizedMessage { get; set; }

    public SortStabilization Stabilization { get; set; }
}
