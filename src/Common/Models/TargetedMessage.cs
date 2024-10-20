﻿namespace ZeroMQPubSubSample.Common.Models;

/// <summary>
/// Represents a targeted message that includes a key, value, and destination.
/// </summary>
/// <remarks>
/// The <see cref="TargetedMessage"/> class inherits from the <see cref="Message"/> class and adds
/// an additional property called <see cref="Destination"/> to indicate where the message is intended to go.
/// </remarks>
public sealed class TargetedMessage : Message
{
    /// <summary>
    /// Gets the destination of the message.
    /// </summary>
    public Destination Destination { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TargetedMessage"/> class.
    /// </summary>
    /// <param name="key">The unique identifier for the message.</param>
    /// <param name="value">The content of the message.</param>
    /// <param name="destination">The destination or target of the message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="destination"/> is <c>null</c>.
    /// </exception>
    public TargetedMessage(Key key, Payload value, Destination destination) : base(key, value)
    {
        ArgumentNullException.ThrowIfNull(destination);
        Destination = destination;
    }
}
