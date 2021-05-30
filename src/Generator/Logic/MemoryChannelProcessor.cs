using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Logic
{
    public class MemoryChannelProcessor : IMemoryChannelProcessor
    {
        public MemoryChannelProcessor(
                                        ILogger<MemoryChannelProcessor> logger,
                                        IMessageMemoryChannel channel,
                                        IMessageSender sender
                                     )
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (channel is null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            _logger = logger;
            _channel = channel;
            _sender = sender;

            _logger.LogInformation("Memory channel processor created.");
        }

        public async Task ProcessAsync(CancellationToken ct)
        {
            await foreach (var msg in _channel.ReadAllAsync(ct))
            {
                _sender.SendMessage(msg);
            }
        }

        private readonly ILogger<MemoryChannelProcessor> _logger;
        private readonly IMessageMemoryChannel _channel;
        private readonly IMessageSender _sender;
    }
}
