namespace ZeroMQPubSubSample.Generator.Logic.Configuration;

/// <summary>
/// Configuration for <see cref="DataGenerator"/>.
/// </summary>
public sealed class DataGeneratorConfiguration
{
    /// <summary>
    /// Data generation period.
    /// </summary>
    public TimeSpan GenerationPeriodSeconds { get; init; }

    /// <summary>
    /// Data generation task marker.
    /// </summary>
    public long TaskId { get; init; }

    /// <summary>
    /// Target queue.
    /// </summary>
    public required string Destination { get; init; }
}
