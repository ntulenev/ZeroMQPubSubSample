namespace ZeroMQPubSubSample.Generator.Logic.Configuration
{
    /// <summary>
    /// Configuration for <see cref="MessageMemoryChannel"/>
    /// </summary>
    public class MessageMemoryChannelConfiguration
    {
        /// <summary>
        /// In-memory channel max size.
        /// </summary>
        public int Capacity { get; set; }
    }
}
