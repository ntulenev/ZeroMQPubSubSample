﻿using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Service.Services;

/// <summary>
/// Hosted service that generates test data messages.
/// </summary>
public sealed class GenerationService : IHostedService
{
    public GenerationService(
                         ILogger<GenerationService> logger,
                         IHostApplicationLifetime hostApplicationLifetime,
                         IEnumerable<IDataGenerator> generators
                                )
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(hostApplicationLifetime);
        ArgumentNullException.ThrowIfNull(generators);

        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _generators = generators;

        _logger.LogDebug("GenerationService created.");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var generationTasks = _generators
                        .Select(x =>
                                    Task.Run(async () => await x.GenerateDataAsync(_hostApplicationLifetime.ApplicationStopping)
                                                                .ConfigureAwait(false)
                                            )
                               );

            _ = generationTasks.Select(x =>
                        x.ContinueWith(x => _hostApplicationLifetime.StopApplication(), TaskContinuationOptions.OnlyOnFaulted));

            _stopper = Task.WhenAll(generationTasks);
        }
        catch (Exception ex)
        {
            _logger?.LogCritical(ex, "Error starting GenerationService.");
            _hostApplicationLifetime.StopApplication();
        }

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping service.");
        try
        {
            await _stopper.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            //Skip
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on stopping service");
        }
    }

    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IEnumerable<IDataGenerator> _generators;
    private Task _stopper = default!;
}
