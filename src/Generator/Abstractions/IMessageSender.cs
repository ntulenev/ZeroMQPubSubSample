using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Abstractions;

/// <summary>
/// Defines a contract for sending messages to external systems or services.
/// </summary>
public interface IMessageSender
{
    /// <summary>
    /// Asynchronously sends a message to an external system.
    /// </summary>
    /// <param name="message">The <see cref="TargetedMessage"/> to be sent.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to signal the cancellation of the operation if necessary.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous send operation.</returns>
    public Task SendMessageAsync(TargetedMessage message, CancellationToken ct);
}

