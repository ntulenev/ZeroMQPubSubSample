namespace ZeroMQPubSubSample.Common.Models
{
    /// <summary>
    /// Represents message with targeted topic.
    /// </summary>
    public record TargetedMessage(long Key, string Value, string Destination) : Message(Key, Value);
}
