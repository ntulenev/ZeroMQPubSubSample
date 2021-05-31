namespace ZeroMQPubSubSample.Generator.Logic.Configuration
{
    /// <summary>
    /// Configuration for <see cref="MessageSender"/>
    /// </summary>
    public class MessageSenderConfiguration
    {
        /// <summary>
        /// Limit on the maximum number of outstanding messages ØMQ shall queue in memory.
        /// </summary>
        public int SendHighWatermark { get; set; }

        /// <summary>
        /// NetMQ tcp socket address.
        /// </summary>
        public string Address { get; set; } = default!;
    }
}
