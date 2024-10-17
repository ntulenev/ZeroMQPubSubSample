namespace ZeroMQPubSubSample.Generator.Logic.Configuration;

/// <summary>
/// Configuration for <see cref="DestinationSender/>.
/// </summary>
public sealed class DestinationSenderConfiguration
{
    /// <summary>
    /// Limit on the maximum number of outstanding messages ØMQ shall queue in memory.
    /// </summary>
    public int SendHighWatermark { get; init; }

    /// <summary>
    /// NetMQ tcp socket address.
    /// </summary>
    public required string Address { get; init; }
}
