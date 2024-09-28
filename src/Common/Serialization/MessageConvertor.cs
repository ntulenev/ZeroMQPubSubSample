using Domain = ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Common.Serialization;

/// <summary>
/// Provides conversion methods between transport and domain message models.
/// </summary>
public static class MessageConvertor
{
    /// <summary>
    /// Converts a transport layer <see cref="Message"/> to a domain layer <see cref="Domain.Message"/>.
    /// </summary>
    /// <param name="msg">The transport layer message to convert.</param>
    /// <returns>A domain layer message.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided message is null.</exception>
    public static Domain.Message FromTransport(this Message msg)
    {
        ArgumentNullException.ThrowIfNull(msg);

        return new Domain.Message(msg.Key, msg.Value);
    }

    /// <summary>
    /// Converts a domain layer <see cref="Domain.Message"/> to a transport layer <see cref="Message"/>.
    /// </summary>
    /// <param name="msg">The domain layer message to convert.</param>
    /// <returns>A transport layer message.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided message is null.</exception>
    public static Message ToTransport(this Domain.Message msg)
    {
        ArgumentNullException.ThrowIfNull(msg);

        return new Message
        {
            Key = msg.Key,
            Value = msg.Value
        };
    }
}

