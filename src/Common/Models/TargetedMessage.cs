namespace ZeroMQPubSubSample.Common.Models;

/// <summary>
/// Represents a targeted message that includes a key, value, and destination.
/// </summary>
/// <param name="Key">The unique key associated with the message.</param>
/// <param name="Value">The content or value of the message.</param>
/// <param name="Destination">The target destination for the message.</param>
public sealed record TargetedMessage(long Key, string Value, string Destination) : Message(Key, Value);
