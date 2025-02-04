﻿namespace ExtraDry.Core;

/// <summary>
/// Standard payload for hierarchy controllers endpoints that return paged results.
/// </summary>
public class PageHierarchyQuery : HierarchyQuery, IPageQuery
{
    /// <inheritdoc cref="IPageQuery.Skip" />
    public int Skip { get; set; }

    /// <inheritdoc cref="IPageQuery.Take" />
    public int Take {
        get => take <= 0 ? DefaultTake : take;
        set => take = value;
    }

    private int take;

    /// <inheritdoc cref="IPageQuery.Token" />
    public string? Token { get; set; }

    /// <summary>
    /// The default number of items to take if none provided.
    /// </summary>
    public const int DefaultTake = 100;
}
