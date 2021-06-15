using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Logic
{
    /// <inheritdoc/>
    public class MemoryChannelProcessor : IMemoryChannelProcessor
    {
        /// <summary>
        /// Creates <see cref="MemoryChannelProcessor"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="channel">Channel with messages.</param>
        /// <param name="sender">Message sender.</param>
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

        /// <inheritdoc/>
        public async Task ProcessAsync(CancellationToken ct)
        {
            try
            {
                await foreach (var msg in _channel.ReadAllAsync(ct).ConfigureAwait(false))
                {
                    _logger.LogDebug("Sending message {message}", msg);
                    await _sender.SendMessageAsync(msg, ct).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                // Skip
            }
        }

        private readonly ILogger<MemoryChannelProcessor> _logger;
        private readonly IMessageMemoryChannel _channel;
        private readonly IMessageSender _sender;
    }
}
