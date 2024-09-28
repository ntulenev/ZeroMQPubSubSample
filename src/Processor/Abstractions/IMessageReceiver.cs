using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Processor.Abstractions;

/// <summary>
/// Receives messages from zeroMQ.
/// </summary>
public interface IMessageReceiver
{
    /// <summary>
    /// Returns messages from zeroMQ as <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    public IAsyncEnumerable<Message> ReceiveAsync(CancellationToken ct);
}
