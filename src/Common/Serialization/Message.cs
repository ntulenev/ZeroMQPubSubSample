using Newtonsoft.Json;

namespace ZeroMQPubSubSample.Common.Serialization;

/// <summary>
/// Represents a message with a key and a value to be serialized as JSON.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public sealed class Message
{
    /// <summary>
    /// Gets the unique key associated with the message.
    /// </summary>
    [JsonProperty("key")]
    [JsonRequired]
    public required long Key { get; init; }

    /// <summary>
    /// Gets the content or value of the message.
    /// </summary>
    [JsonProperty("value")]
    [JsonRequired]
    public required string Value { get; init; }
}

