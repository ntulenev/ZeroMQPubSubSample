using System.Threading.Channels;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic.Configuration;

namespace ZeroMQPubSubSample.Generator.Logic
{
    /// <inheritdoc/>
    public class MessageMemoryChannel : IMessageMemoryChannel
    {
        /// <summary>
        /// Creates <see cref="MessageMemoryChannel"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="options">Channel settings.</param>
        public MessageMemoryChannel(
                                    ILogger<MessageMemoryChannel> logger,
                                    IOptions<MessageMemoryChannelConfiguration> options)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Value is null)
            {
                throw new ArgumentException("Options is not set.", nameof(options));
            }

            _logger = logger;
            _channel = Channel.CreateBounded<TargetedMessage>(options.Value.Capacity);

            _logger.LogDebug("Memory channel created with capacity = {Capacity}.", options.Value.Capacity);
        }

        /// <inheritdoc/>
        public async ValueTask WriteAsync(TargetedMessage message, CancellationToken ct)
        {
            await _channel.Writer.WriteAsync(message, ct).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TargetedMessage> ReadAllAsync(CancellationToken ct)
        {
            return _channel.Reader.ReadAllAsync(ct);
        }

        private readonly Channel<TargetedMessage> _channel;
        private readonly ILogger<MessageMemoryChannel> _logger;
    }
}
