using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Processor.Abstractions;

/// <summary>
/// Represents a service that processes messages received from an <see cref="IMessageReceiver"/>.
/// </summary>
public interface IMessageProcessor
{
    /// <summary>
    /// Processes the given message asynchronously.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> to be processed.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ProcessAsync(Message message, CancellationToken ct);
}
