using Newtonsoft.Json;

namespace ZeroMQPubSubSample.Common.Serialization;

/// <summary>
/// Transport message model.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public sealed class Message
{
    /// <summary>
    /// Message key.
    /// </summary>
    [JsonProperty("key")]
    [JsonRequired]
    public required long Key { get; init; }

    /// <summary>
    /// Message value.
    /// </summary>
    [JsonProperty("value")]
    [JsonRequired]
    public required string Value { get; init; }
}
