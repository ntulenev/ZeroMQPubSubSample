using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Processor.Abstractions;

/// <summary>
/// Process messages from <see cref="IMessageReceiver"/>.
/// </summary>
public interface IMessageProcessor
{
    /// <summary>
    /// Process message.
    /// </summary>
    public Task ProcessAsync(Message message, CancellationToken ct);
}
