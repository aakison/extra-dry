namespace ExtraDry.Server.Agents;

/// <summary>
/// Represents an interface for jobs that can be run on a specific schedule.
/// </summary>
public interface ICronJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken);
}
