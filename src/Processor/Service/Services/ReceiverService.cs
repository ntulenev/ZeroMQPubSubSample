using ZeroMQPubSubSample.Processor.Abstractions;

namespace ZeroMQPubSubSample.Processor.Service.Services;

/// <summary>
/// Service responsible for receiving and processing messages asynchronously.
/// Implements the <see cref="IHostedService"/> interface to start and stop the service within a host.
/// </summary>
internal sealed class ReceiverService : IHostedService
{
    /// <summary>
    /// Initializes a new instance of the ReceiverService class with the specified logger, application lifetime manager,
    /// and receive handler.
    /// </summary>
    /// <param name="logger">The logger used to record informational and error messages for the service. Cannot be null.</param>
    /// <param name="hostApplicationLifetime">The application lifetime manager that notifies the
    /// service of application start and stop events. Cannot be null.</param>
    /// <param name="handler">The handler responsible for processing received messages. Cannot be null.</param>
    public ReceiverService(
        ILogger<ReceiverService> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        IReceiveHandler handler)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(hostApplicationLifetime);
        ArgumentNullException.ThrowIfNull(handler);

        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _handler = handler;

        _logger.LogInformation("ReceiverService created.");
    }

    /// <summary>
    /// Starts the service and begins asynchronously receiving and processing messages.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that the operation should be canceled.</param>
    /// <returns>A completed <see cref="Task"/> when the startup is complete.</returns>
    /// <remarks>
    /// The service starts a task that listens for messages from the <see cref="IMessageReceiver"/> and processes them
    /// with the <see cref="IMessageProcessor"/>. If an error occurs, the application is stopped.
    /// </remarks>
    public Task StartAsync(CancellationToken cancellationToken)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
#pragma warning disable CA2016 // Forward the 'CancellationToken' parameter to methods
            _stopper = Task.Run(async () =>
            await _handler.HandleAsync(_hostApplicationLifetime.ApplicationStopping).ConfigureAwait(false));
#pragma warning restore CA2016 // Forward the 'CancellationToken' parameter to methods

#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
            _ = _stopper.ContinueWith(x => _hostApplicationLifetime.StopApplication(), TaskContinuationOptions.OnlyOnFaulted);
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler
        }
        catch (Exception ex)
        {
            _logger?.LogCritical(ex, "Error starting ReceiverService.");
            _hostApplicationLifetime.StopApplication();
        }
#pragma warning restore CA1031 // Do not catch general exception types

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the service and ensures any ongoing message processing is completed.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that the operation should be canceled.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous stop operation.</returns>
    /// <remarks>
    /// If the service is already processing a message, it waits for the processing to complete before stopping.
    /// </remarks>
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
    private readonly IReceiveHandler _handler;
    private Task _stopper = default!;
}

