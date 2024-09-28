using Domain = ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Common.Serialization;

/// <summary>
/// Helper that converts Domain and Transport models.
/// </summary>
public static class MessageConvertor
{
    /// <summary>
    /// Create <see cref="Domain.Message"/> from <see cref="Message"/>.
    /// </summary>
    /// <param name="msg">Domain message model.</param>
    /// <returns>Transport message model.</returns>
    public static Domain.Message FromTransport(this Message msg)
    {
        if (msg is null)
        {
            throw new ArgumentNullException(nameof(msg));
        }

        if (msg.Value is null)
        {
            throw new ArgumentException("Message Value is not set.", nameof(msg));
        }

        if (string.IsNullOrWhiteSpace(msg.Value))
        {
            throw new ArgumentException("Message Value is empty of contains only whitespaces.", nameof(msg));
        }

        return new Domain.Message(msg.Key, msg.Value);
    }

    /// <summary>
    /// Create <see cref="Message"/> from <see cref="Domain.Message"/>.
    /// </summary>
    /// <param name="msg">Transport message model.</param>
    /// <returns>Domain message model.</returns>
    public static Message ToTransport(this Domain.Message msg)
    {
        if (msg is null)
        {
            throw new ArgumentNullException(nameof(msg));
        }

        return new Message
        {
            Key = msg.Key,
            Value = msg.Value
        };
    }
}
