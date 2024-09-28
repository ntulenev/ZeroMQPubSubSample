namespace ZeroMQPubSubSample.Common.Models;

/// <summary>
/// Represents a message that contains a key and a value.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets the unique key of the message.
    /// </summary>
    public long Key { get; }

    /// <summary>
    /// Gets the value of the message.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Message"/> class.
    /// </summary>
    /// <param name="key">The unique identifier for the message.</param>
    /// <param name="value">The content of the message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is empty or contains only whitespace characters.
    /// </exception>
    public Message(long key, string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Message Value is empty or contains only whitespaces.", nameof(value));
        }

        Key = key;
        Value = value;
    }
}
