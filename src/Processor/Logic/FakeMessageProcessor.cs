using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Processor.Abstractions;

namespace ZeroMQPubSubSample.Processor.Logic;

/// <summary>
/// Stub for processing messages.
/// </summary>
public sealed class FakeMessageProcessor : IMessageProcessor
{
    /// <inheritdoc/>
    public FakeMessageProcessor(ILogger<FakeMessageProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;

        _logger.LogDebug("FakeMessageProcessor created.");
    }

    /// <summary>
    /// Processes the specified message asynchronously.
    /// </summary>
    /// <param name="message">The message to be processed. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task ProcessAsync(Message message, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);

        _logger.LogInformation("Processing {Message}", message);
        return Task.CompletedTask;
    }

    private readonly ILogger _logger;
}
