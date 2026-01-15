using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Logic;

/// <summary>
/// Processes messages from an in-memory channel and sends them using the specified message sender.
/// </summary>
/// <remarks>This class is typically used to bridge an in-memory message channel with an external message sender,
/// enabling asynchronous processing and forwarding of messages. Instances of this class are not thread-safe and should
/// be used on a single processing thread.</remarks>
public sealed class MemoryChannelProcessor : IMemoryChannelProcessor
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
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(sender);

        _logger = logger;
        _channel = channel;
        _sender = sender;

        _logger.LogDebug("Memory channel processor created.");
    }

    /// <inheritdoc/>
    public async Task ProcessAsync(CancellationToken ct)
    {
        try
        {
            await foreach (var msg in _channel.ReadAllAsync(ct).ConfigureAwait(false))
            {
                try
                {
                    _logger.LogDebug("Sending message {Message}", msg);
                    await _sender.SendMessageAsync(msg, ct).ConfigureAwait(false);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Error on sending message {Message}", msg);
                    throw;
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Skip
        }
    }

    private readonly ILogger _logger;
    private readonly IMessageMemoryChannel _channel;
    private readonly IMessageSender _sender;
}
