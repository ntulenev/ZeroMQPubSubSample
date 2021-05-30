namespace ZeroMQPubSubSample.Common.Models
{
    public record TargetedMessage(long Key, string Value, string Destination) : Message(Key, Value);
}
