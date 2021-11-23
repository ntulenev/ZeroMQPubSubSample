using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Processor.Abstractions;

namespace ZeroMQPubSubSample.Processor.Logic.Configuration
{
    /// <summary>
    /// Stub for processing messages.
    /// </summary>
    public class FakeMessageProcessor : IMessageProcessor
    {
        /// <inheritdoc/>
        public FakeMessageProcessor(ILogger<FakeMessageProcessor> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.LogDebug("FakeMessageProcessor created.");
        }

        public Task ProcessAsync(Message message, CancellationToken ct)
        {
            _logger.LogInformation("Processing {message}", message);
            return Task.CompletedTask;
        }

        private readonly ILogger<FakeMessageProcessor> _logger;
    }
}
