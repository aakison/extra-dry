namespace ExtraDry.Core;

/// <summary>
/// An immutable query object that is used to build the query string for the request.
/// </summary>
public sealed class Query
{
    public string? Filter { get; init; }

    public string? Sort { get; init; }

    public int? Skip { get; init; }

    public int? Take { get; init; }

    public int? Level { get; init; }

    public string[]? Expand { get; init; }

    public string[]? Collapse { get; init; }
}
