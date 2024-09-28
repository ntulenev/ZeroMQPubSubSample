using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Processor.Abstractions;

namespace ZeroMQPubSubSample.Processor.Logic.Configuration;

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

    public Task ProcessAsync(Message message, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);

        _logger.LogInformation("Processing {message}", message);
        return Task.CompletedTask;
    }

    private readonly ILogger _logger;
}
