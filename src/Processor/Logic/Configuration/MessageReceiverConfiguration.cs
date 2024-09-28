namespace ZeroMQPubSubSample.Processor.Logic.Configuration;

/// <summary>
/// Configuration for <see cref="MessageReceiverConfiguration"/>.
/// </summary>
public sealed class MessageReceiverConfiguration
{
    /// <summary>
    /// NetMQ tcp socket address.
    /// </summary>
    public required string Address { get; init; }

    /// <summary>
    /// Limit on the maximum number of incoming messages ØMQ shall queue in memory.
    /// </summary>
    public int ReceiveHighWatermark { get; init; }

    /// <summary>
    /// Targeted queue.
    /// </summary>
    public required string Topic { get; init; }
}
