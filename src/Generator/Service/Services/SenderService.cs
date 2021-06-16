using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Service.Services
{
    /// <summary>
    /// Hosted service that sends test data messages.
    /// </summary>
    public class SenderService : IHostedService
    {
        public SenderService(ILogger<SenderService> logger,
                             IHostApplicationLifetime hostApplicationLifetime,
                             IMemoryChannelProcessor processor
                                    )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));

            _logger.LogDebug("SenderService created.");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _stopper = Task.Run(async () => await _processor
                                            .ProcessAsync(_hostApplicationLifetime.ApplicationStopping).ConfigureAwait(false));
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

        private readonly ILogger<SenderService> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IMemoryChannelProcessor _processor;
        private Task _stopper = default!;
    }
}
