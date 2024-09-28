using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Abstractions;

/// <summary>
///Sends data to other systems.
/// </summary>
public interface IMessageSender
{
    /// <summary>
    /// Sends message to outer systems.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns></returns>
    public Task SendMessageAsync(TargetedMessage message, CancellationToken ct);
}
