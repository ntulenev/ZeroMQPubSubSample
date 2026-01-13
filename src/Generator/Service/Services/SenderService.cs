using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Service.Services;

/// <summary>
/// Hosted service that sends test data messages.
/// </summary>
internal sealed class SenderService : IHostedService
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
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
#pragma warning disable CA2016 // Forward the 'CancellationToken' parameter to methods
            _stopper = Task.Run(async () => await _processor
                                        .ProcessAsync(_hostApplicationLifetime.ApplicationStopping).ConfigureAwait(false)
                                );
#pragma warning restore CA2016 // Forward the 'CancellationToken' parameter to methods
#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
            _ = _stopper.ContinueWith(x => _hostApplicationLifetime.StopApplication(), TaskContinuationOptions.OnlyOnFaulted);
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler
        }
        catch (Exception ex)
        {
            _logger?.LogCritical(ex, "Error starting SenderService.");
            _hostApplicationLifetime.StopApplication();
        }
#pragma warning restore CA1031 // Do not catch general exception types

        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping service.");
#pragma warning disable CA1031 // Do not catch general exception types
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
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IMemoryChannelProcessor _processor;
    private Task _stopper = default!;
}
