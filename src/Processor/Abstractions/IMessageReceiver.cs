using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Processor.Abstractions;

/// <summary>
/// Defines a service that receives messages from a ZeroMQ connection.
/// </summary>
public interface IMessageReceiver
{
    /// <summary>
    /// Asynchronously receives messages from ZeroMQ and returns them as an <see cref="IAsyncEnumerable{T}"/> stream.
    /// </summary>
    /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="IAsyncEnumerable{Message}"/> representing the stream of incoming messages.</returns>
    public IAsyncEnumerable<Message> ReceiveAsync(CancellationToken ct);
}
