using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic.Configuration;

namespace ZeroMQPubSubSample.Generator.Logic
{
    /// <inheritdoc/>
    public class DataGenerator : IDataGenerator
    {
        /// <summary>
        /// Creates <see cref="DataGenerator"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="options">Configuration params.</param>
        /// <param name="channel">In-memory queue.</param>
        public DataGenerator(
                             ILogger<DataGenerator> logger,
                             IOptions<DataGeneratorConfiguration> options,
                             IMessageMemoryChannel channel)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (channel is null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            if (options.Value is null)
            {
                throw new ArgumentException("Options is not set.", nameof(options));
            }

            _channel = channel;
            _config = options.Value;
            _logger = logger;

            _logger.LogDebug("Data generator {TaskId} created.", _config.TaskId);
        }

        /// <inheritdoc/>
        public async Task GenerateDataAsync(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    await Task.Delay(_config.GenerationPeriodSeconds, ct).ConfigureAwait(false);

                    var msg = new TargetedMessage(_config.TaskId, Guid.NewGuid().ToString(), _config.Destination);

                    _logger.LogInformation("Creating new data {message}", msg);

                    await _channel.WriteAsync(msg, ct).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                // Skip
            }
        }

        private readonly ILogger<DataGenerator> _logger;
        private readonly IMessageMemoryChannel _channel;
        private readonly DataGeneratorConfiguration _config;
    }
}
