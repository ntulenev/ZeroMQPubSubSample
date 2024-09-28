﻿namespace ZeroMQPubSubSample.Generator.Logic.Configuration;

/// <summary>
/// Configuration for <see cref="DataGenerator"/>.
/// </summary>
public sealed class DataGeneratorConfiguration
{
    /// <summary>
    /// Data generation period.
    /// </summary>
    public TimeSpan GenerationPeriodSeconds { get; set; }

    /// <summary>
    /// Data generation task marker.
    /// </summary>
    public long TaskId { get; set; }

    /// <summary>
    /// Target queue.
    /// </summary>
    public string Destination { get; set; } = default!;
}
