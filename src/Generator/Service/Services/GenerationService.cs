using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Service.Services;

/// <summary>
/// Service responsible for coordinating data generation using multiple <see cref="IDataGenerator"/> instances.
/// Implements the <see cref="IHostedService"/> interface to start and stop the service within a host.
/// </summary>
internal sealed class GenerationService : IHostedService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenerationService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance used for logging messages.</param>
    /// <param name="hostApplicationLifetime">Provides events for the application
    /// lifecycle and allows for graceful shutdowns.</param>
    /// <param name="generators">A collection of <see cref="IDataGenerator"/> instances that generate data asynchronously.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any of the parameters (<paramref name="logger"/>, <paramref name="hostApplicationLifetime"/>, 
    /// or <paramref name="generators"/>) is <c>null</c>.
    /// </exception>
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

    /// <summary>
    /// Starts the service and begins executing data generation tasks concurrently.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that the operation should be canceled.</param>
    /// <returns>A completed <see cref="Task"/> when the startup process is complete.</returns>
    /// <remarks>
    /// The service starts multiple asynchronous data generation tasks using the provided <see cref="IDataGenerator"/> instances.
    /// If any task fails, the service triggers an application shutdown.
    /// </remarks>
    public Task StartAsync(CancellationToken cancellationToken)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var generationTasks = _generators
                        .Select(x =>
                                    Task.Run(async () => await x.GenerateDataAsync(_hostApplicationLifetime.ApplicationStopping)
                                                                .ConfigureAwait(false)
                                            )
                               );

#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
            _ = generationTasks.Select(x =>
                        x.ContinueWith(x => _hostApplicationLifetime.StopApplication(), TaskContinuationOptions.OnlyOnFaulted));
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler

            _stopper = Task.WhenAll(generationTasks);
        }
        catch (Exception ex)
        {
            _logger?.LogCritical(ex, "Error starting GenerationService.");
            _hostApplicationLifetime.StopApplication();
        }
#pragma warning restore CA1031 // Do not catch general exception types

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the service and ensures that all ongoing data generation tasks complete before termination.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that the operation should be canceled.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous stop operation.</returns>
    /// <remarks>
    /// The service waits for all data generation tasks to complete before stopping. If an exception occurs during stopping,
    /// it is logged.
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
            _logger.LogError(ex, "Error on stopping service");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IEnumerable<IDataGenerator> _generators;
    private Task _stopper = default!;
}

