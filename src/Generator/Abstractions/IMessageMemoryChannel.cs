using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Abstractions;

/// <summary>
/// Represents an in-memory message queue for managing asynchronous message processing.
/// </summary>
public interface IMessageMemoryChannel
{
    /// <summary>
    /// Asynchronously adds a new message to the in-memory queue.
    /// </summary>
    /// <param name="message">The <see cref="TargetedMessage"/> to be added to the queue.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> used to cancel the operation if needed.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous write operation.</returns>
    ValueTask WriteAsync(TargetedMessage message, CancellationToken ct);

    /// <summary>
    /// Asynchronously reads all messages from the in-memory queue as an asynchronous stream.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation if needed.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that asynchronously iterates over all 
    /// <see cref="TargetedMessage"/> items in the queue.</returns>
    IAsyncEnumerable<TargetedMessage> ReadAllAsync(CancellationToken cancellationToken = default);
}

