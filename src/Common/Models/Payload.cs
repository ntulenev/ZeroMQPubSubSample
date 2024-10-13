namespace ZeroMQPubSubSample.Common.Models;

/// <summary>
/// Represents a sealed class that holds a Message payload value.
/// </summary>
public sealed class Payload
{
    /// <summary>
    /// Gets the value of the payload.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Payload"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the payload.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="value"/> 
    /// is empty or consists only of white-space characters.</exception>
    public Payload(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Payload value is empty or contains only whitespaces.", nameof(value));
        }

        Value = value;
    }
}

