﻿using ExtraDry.Core.Parser.Internal;
using ExtraDry.Server.Internal;

namespace ExtraDry.Server;

public class StatisticsQueryable<T> : BaseQueryable<T>
{
    /// <inheritdoc cref="IFilteredQueryable{T}.ToStatistics" />
    public Statistics<T> ToStatistics()
    {
        var stats = new Statistics<T> {
            Distributions = [],
            Filter = TrimFilter(Query.Filter),
        };
        var description = new ModelInfo(typeof(T));

        foreach(var statProp in description.StatisticsProperties) {
            var statsQuery = FilteredQuery
                .GroupBy(statProp)
                .Select(e => new CountInfo(e.Key, e.Count()));
            var items = new List<CountInfo>();
            items.AddRange(statsQuery);
            stats.Distributions.Add(new DataDistribution(statProp.Property.Name, items.ToDictionary(e => e.Key, e => e.Count)));
        }
        return stats;
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToStatisticsAsync(CancellationToken)" />
    public async Task<Statistics<T>> ToStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var stats = new Statistics<T> {
            Distributions = [],
            Filter = TrimFilter(Query.Filter),
        };
        var description = new ModelInfo(typeof(T));

        foreach(var statProp in description.StatisticsProperties) {
            var statsQuery = FilteredQuery
                .GroupBy(statProp)
                .Select(e => new CountInfo(e.Key, e.Count()));

            var items = await ToListAsync(statsQuery, cancellationToken);

            stats.Distributions.Add(new DataDistribution(statProp.Property.Name, items.ToDictionary(e => e.Key, e => e.Count)));
        }
        return stats;
    }

    private static string? TrimFilter(string? filter)
    {
        return filter?.Trim() ?? "";
    }
}
