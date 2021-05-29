using Newtonsoft.Json;

namespace ZeroMQPubSubSample.Common.Serialization
{
    /// <summary>
    /// Transport message model.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Message
    {
        /// <summary>
        /// Message key.
        /// </summary>
        [JsonProperty("key")]
        [JsonRequired]
        public long Key { get; set; }

        /// <summary>
        /// Message value.
        /// </summary>
        [JsonProperty("value")]
        [JsonRequired]
        public string Value { get; set; } = default!;
    }
}
