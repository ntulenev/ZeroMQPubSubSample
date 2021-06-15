namespace ZeroMQPubSubSample.Processor.Logic.Configuration
{
    public class MessageReceiverConfiguration
    {
        /// <summary>
        /// NetMQ tcp socket address.
        /// </summary>
        public string Address { get; set; } = default!;

        /// <summary>
        /// Limit on the maximum number of incomming messages ØMQ shall queue in memory.
        /// </summary>
        public int ReceiveHighWatermark { get; set; }

        /// <summary>
        /// Target queue
        /// </summary>
        public string Topic { get; set; } = default!;
    }
}
