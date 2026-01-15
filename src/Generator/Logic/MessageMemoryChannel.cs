using System.Threading.Channels;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic.Configuration;

namespace ZeroMQPubSubSample.Generator.Logic;

/// <summary>
/// Provides an in-memory message channel for asynchronously writing and reading targeted messages.
/// </summary>
/// <remarks>This class implements a bounded channel for message passing within the application. It is typically
/// used to facilitate communication between producers and consumers in memory, without persistent storage. The channel
/// capacity is configured via the provided options. This type is not thread-safe for external mutation; concurrent read
/// and write operations are supported through the channel's own synchronization.</remarks>
public sealed class MessageMemoryChannel : IMessageMemoryChannel
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
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);

        if (options.Value is null)
        {
            throw new ArgumentException("Options is not set.", nameof(options));
        }

        _logger = logger;
        _channel = Channel.CreateBounded<TargetedMessage>(options.Value.Capacity);

        _logger.LogDebug("Memory channel created with capacity = {Capacity}.", options.Value.Capacity);
    }

    /// <inheritdoc/>
    public async ValueTask WriteAsync(TargetedMessage message, CancellationToken ct) =>
        await _channel.Writer.WriteAsync(message, ct).ConfigureAwait(false);

    /// <inheritdoc/>
    public IAsyncEnumerable<TargetedMessage> ReadAllAsync(CancellationToken cancellationToken) =>
        _channel.Reader.ReadAllAsync(cancellationToken);

    private readonly Channel<TargetedMessage> _channel;
    private readonly ILogger _logger;
}
