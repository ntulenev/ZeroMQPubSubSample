using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Logic;

/// <inheritdoc/>
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
