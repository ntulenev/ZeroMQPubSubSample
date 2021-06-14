namespace ZeroMQPubSubSample.Processor.Logic.Configuration
{
    class MessageReceiverConfiguration
    {
        /// <summary>
        /// NetMQ tcp socket address.
        /// </summary>
        public string Address { get; set; } = default!;
    }
}
