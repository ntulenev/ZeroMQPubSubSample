namespace ZeroMQPubSubSample.Common.Models;

/// <summary>
/// Represents a message that contains a key and a value.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets the unique key of the message.
    /// </summary>
    public Key Key { get; }

    /// <summary>
    /// Gets the payload of the message.
    /// </summary>
    public Payload Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Message"/> class.
    /// </summary>
    /// <param name="key">The unique identifier for the message.</param>
    /// <param name="value">The content of the message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is <c>null</c>.
    /// </exception>
    public Message(Key key, Payload value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Key = key;
        Value = value;
    }

    /// <summary>
    /// Returns a string that represents the current message.
    /// </summary>
    /// <returns>A string that includes the message key and value.</returns>
    public override string ToString() => $"Message [Key={Key}, Value={Value.Value}]";
}
