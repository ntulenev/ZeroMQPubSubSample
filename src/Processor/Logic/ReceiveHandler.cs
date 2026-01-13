
using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Processor.Abstractions;

namespace ZeroMQPubSubSample.Processor.Logic;

/// <summary>
/// A sealed class that handles receiving and processing messages.
/// </summary>
public sealed class ReceiveHandler : IReceiveHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiveHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger used to log operations and events.</param>
    /// <param name="receiver">The message receiver responsible for receiving messages.</param>
    /// <param name="processor">The message processor responsible for processing messages.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any of the arguments are null.
    /// </exception>
    public ReceiveHandler(
        ILogger<ReceiveHandler> logger,
        IMessageReceiver receiver,
        IMessageProcessor processor)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(receiver);
        ArgumentNullException.ThrowIfNull(processor);

        _logger = logger;
        _processor = processor;
        _receiver = receiver;

        _logger.LogInformation("ReceiveHandler created.");
    }

    /// <summary>
    /// Handles the received messages asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task HandleAsync(CancellationToken cancellationToken)
    {
        await foreach (var message in _receiver.ReceiveAsync(cancellationToken)
                                              .ConfigureAwait(false))
        {
            _logger.LogDebug("Process message {Message}.", message);
            await _processor.ProcessAsync(message, cancellationToken).ConfigureAwait(false);
        }
    }

    private readonly ILogger _logger;
    private readonly IMessageReceiver _receiver;
    private readonly IMessageProcessor _processor;
}
