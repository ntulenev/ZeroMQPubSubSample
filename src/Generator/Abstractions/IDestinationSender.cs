using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Abstractions;

/// <summary>
/// Represents a sender that can send messages to a specified destination.
/// </summary>
public interface IDestinationSender : IDisposable
{
    /// <summary>
    /// Sends a message to the specified destination.
    /// </summary>
    /// <param name="destination">The destination to which the message should be sent.</param>
    /// <param name="message">The message to send to the destination.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="destination"/> or <paramref name="message"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if the method is called after the sender has been disposed.
    /// </exception>
    void Send(Destination destination, string message);
}
