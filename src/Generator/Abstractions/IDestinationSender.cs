using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Abstractions;


/// <summary>
/// Defines an interface for sending messages of type <typeparamref name="T"/> to a specified <see cref="Destination"/>.
/// </summary>
/// <typeparam name="T">The type of message to be sent.</typeparam>
public interface IDestinationSender<T> : IDisposable
{
    /// <summary>
    /// Sends a message of type <typeparamref name="T"/> to the specified <see cref="Destination"/>.
    /// </summary>
    /// <param name="destination">The destination where the message should be sent.</param>
    /// <param name="message">The message to send.</param>
    void Send(Destination destination, T message);
}

