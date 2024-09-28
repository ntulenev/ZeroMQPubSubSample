namespace ZeroMQPubSubSample.Common.Models;

/// <summary>
/// Represents a message with a key and a value.
/// </summary>
/// <param name="Key">The unique key associated with the message.</param>
/// <param name="Value">The content or value of the message.</param>
public record Message(long Key, string Value);
