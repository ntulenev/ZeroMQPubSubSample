using ZeroMQPubSubSample.Processor.Abstractions;

namespace ZeroMQPubSubSample.Processor.Service.Services;

/// <summary>
/// Service responsible for receiving and processing messages asynchronously.
/// Implements the <see cref="IHostedService"/> interface to start and stop the service within a host.
/// </summary>
public sealed class ReceiverService : IHostedService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiverService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance used for logging messages.</param>
    /// <param name="hostApplicationLifetime">Provides events for the application 
    /// lifecycle and allows for graceful shutdowns.</param>
    /// <param name="receiver">The message receiver that retrieves messages asynchronously.</param>
    /// <param name="processor">The message processor that handles the processing of the received messages.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any of the parameters (<paramref name="logger"/>, <paramref name="hostApplicationLifetime"/>, 
    /// <paramref name="receiver"/>, or <paramref name="processor"/>) is <c>null</c>.
    /// </exception>
    public ReceiverService(
        ILogger<ReceiverService> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        IMessageReceiver receiver,
        IMessageProcessor processor
    )
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(hostApplicationLifetime);
        ArgumentNullException.ThrowIfNull(receiver);
        ArgumentNullException.ThrowIfNull(processor);

        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _processor = processor;
        _receiver = receiver;

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
        try
        {
            _stopper = Task.Run(async () =>
            {
                //TODO Change on handler;
                await foreach (var message in _receiver.ReceiveAsync(_hostApplicationLifetime.ApplicationStopping)
                                                      .ConfigureAwait(false))
                {
                    _logger.LogDebug("Process message {message}.", message);
                    await _processor.ProcessAsync(message, _hostApplicationLifetime.ApplicationStopping);
                }
            });
            _stopper.ContinueWith(x => _hostApplicationLifetime.StopApplication(), TaskContinuationOptions.OnlyOnFaulted);
        }
        catch (Exception ex)
        {
            _logger?.LogCritical(ex, "Error starting ReceiverService.");
            _hostApplicationLifetime.StopApplication();
        }

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
    private readonly IMessageReceiver _receiver;
    private readonly IMessageProcessor _processor;
    private Task _stopper = default!;
}

