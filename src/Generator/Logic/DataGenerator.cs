using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic.Configuration;

namespace ZeroMQPubSubSample.Generator.Logic;

/// <inheritdoc/>
public sealed class DataGenerator : IDataGenerator
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

        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(channel);

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
            while (true)
            {
                ct.ThrowIfCancellationRequested();

                await Task.Delay(_config.GenerationPeriodSeconds, ct).ConfigureAwait(false);

                var msg = CreateMessage();

                _logger.LogInformation("Creating new data {message}", msg);

                await _channel.WriteAsync(msg, ct).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            // Skip
        }
    }

    private TargetedMessage CreateMessage()
    {
        var payload = Guid.NewGuid().ToString();
        return new TargetedMessage(_config.TaskId, payload, _config.Destination);
    }

    private readonly ILogger _logger;
    private readonly IMessageMemoryChannel _channel;
    private readonly DataGeneratorConfiguration _config;
}
