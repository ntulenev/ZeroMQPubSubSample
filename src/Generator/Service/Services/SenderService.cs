using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Service.Services;

/// <summary>
/// Hosted service that sends test data messages.
/// </summary>
public sealed class SenderService : IHostedService
{
    public SenderService(ILogger<SenderService> logger,
                         IHostApplicationLifetime hostApplicationLifetime,
                         IMemoryChannelProcessor processor
                                )
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(hostApplicationLifetime);
        ArgumentNullException.ThrowIfNull(processor);

        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _processor = processor;

        _logger.LogDebug("SenderService created.");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _stopper = Task.Run(async () => await _processor
                                        .ProcessAsync(_hostApplicationLifetime.ApplicationStopping).ConfigureAwait(false)
                                );
            _stopper.ContinueWith(x => _hostApplicationLifetime.StopApplication(), TaskContinuationOptions.OnlyOnFaulted);
        }
        catch (Exception ex)
        {
            _logger?.LogCritical(ex, "Error starting SenderService.");
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
            _logger.LogError(ex, "Error on stopping service.");
        }
    }

    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IMemoryChannelProcessor _processor;
    private Task _stopper = default!;
}
